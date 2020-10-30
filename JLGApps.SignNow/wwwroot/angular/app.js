var myApp = angular.module('templateDatagrid', ['summernote', 'ui.bootstrap'])

myApp.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);


myApp.controller('templateController', ['$scope', '$http', function ($scope, $http) {

    $scope.mailboxes = ["earplug_updates@johnsonlawgroup.com", "essure_updates@johnsonlawgroup.com", "ethicontvt@johnsonlawgroup.com", "hips_updates@johnsonlawgroup.com", "herniamesh@johnsonlawgroup.com", "rsp_updates@johnsonlawgroup.com", "rup_updates@johnsonlawgroup.com", "talc_updates@johnsonlawgroup.com", "tdf_updates@johnsonlawgroup.com", "tvm_updates@johnsonlawgroup.com", "txt_updates@johnsonlawgroup.com", "xar_updates@johnsonlawgroup.com", "zan_updates@johnsonlawgroup.com"];
    $scope.selectedMailbox = "earplug_updates@johnsonlawgroup.com";


    sender = document.getElementById('selected_EmailSender').value


    if (sender != "")
        $scope.selectedMailbox = sender;


    $http.get('api/EmailTemplateDetails').then(function (response) {
        $scope.rows = response.data;
        $scope.viewby = 5;
        $scope.totalItems = response.data.length;
        $scope.currentPage = 1;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5;
        document.getElementById("pageCount").nodeValue = $scope.viewby;

    }, function error(response) {

        alert('error');

    });


    $scope.delete = function (row) {
        var templateId = row.TemplateId;
        $http.delete('api/EmailTemplateDetails/' + templateId).then(function success(response) {

            $http.get('api/EmailTemplateDetails').then(function (response) {



                $scope.rows = response.data;
                $scope.viewby = 5;
                $scope.totalItems = response.data.length;
                $scope.itemsPerPage = $scope.viewby;
                $scope.maxSize = 5;
                document.getElementById("pageCount").nodeValue = $scope.viewby;

                Snackbar.show({
                    text: 'Warning: Template has been deleted!',
                    actionTextColor: '#fff',
                    backgroundColor: '#e7515a'
                });

            });

        });
    }


    $scope.templateLoader = function (row) {

        var templateId = row.TemplateId;
        $http.get('api/EmailTemplateDetails/' + templateId).then(function (response) {

            document.getElementById('subject').value = response.data.EmailSubject;
            document.getElementById('recipientEmail').value = response.data.EmailRecipient;
            $('#summernote').summernote('reset');
            $('#summernote').summernote('pasteHTML', response.data.EmailBody);

            var sender = response.data.EmailSender.trim()

            $scope.selectedMailbox = sender;

        })



    }

    $scope.selectedrowValues = function (row) {
        document.getElementById('selectedTemplateId').value = row.TemplateId;
        document.getElementById('selectedTemplateName').value = row.TemplateName;
        document.getElementById('selectedTemplateDocket').value = row.Docket;
        document.getElementById('selectedTemplateCategory').value = row.Category;
      
    }

    $scope.update = function (template) {
        var templateId = $('#selectedTemplateId').val();
        var templateName = $('#selectedTemplateName').val();
        var templateDocket = $('#selectedTemplateDocket').val();
        var templateCategory = $('#selectedTemplateCategory').val();

        document.getElementById("alertUpdateTemplate").style.display = 'none';
        var editorContent = $('#summernote').summernote('code');
        var subject = $('#subject').val();
        var sender = $('#from').val();
        var recipient = $('#recipientEmail').val();
        var xlsxFileAndPath = $('#fileAndPath').text();
        var smsBody = $('#smsBody').val(); ''
        var smsRecipient = $('#smsRecipient').val();
        var dateTime = new Date();
        template.DateCreated = dateTime.toISOString();
        template.EmailBody = editorContent;
        template.email = editorContent;
        template.EmailRecipient = recipient;
        template.EmailSender = sender;
        template.EmailSubject = subject;
        template.SMSBody = smsBody;
        template.SMSRecipient = smsRecipient;
        template.TemplateName = templateName;
        template.Category = templateCategory;
        template.Docket = templateDocket;
        template.templateId = templateId;

        $http.put('api/EmailTemplateDetails/' + templateId, template).then(function success(response) {
            $scope.rows = response.data;

            $http.get('api/EmailTemplateDetails').then(function (response) {

                $scope.rows = response.data;
                $scope.viewby = 5;
                $scope.totalItems = response.data.length;
                $scope.itemsPerPage = $scope.viewby;
                $scope.maxSize = 5;
                document.getElementById("pageCount").nodeValue = $scope.viewby;
                document.getElementById("alertUpdateTemplate").style.display = 'block';
                Snackbar.show({
                    text: 'Success: Template has been updated',
                    actionTextColor: '#fff',
                    backgroundColor: '#51e75b'
                });

                document.getElementById('addTemplateName').value = '';
                document.getElementById('addTemplateCategory').value = '';
                document.getElementById('addTemplateDocument').value = '';
            });

        });
    }



    $scope.add = function (template) {

     
        document.getElementById("alertNewTemplate").style.display = 'none';
        var editorContent = $('#summernote').summernote('code');
        var subject = $('#subject').val();
        var sender = $('#from').val();
        var recipient = $('#recipientEmail').val();
        var xlsxFileAndPath = $('#fileAndPath').text();
        var smsBody = $('#smsBody').val(); ''
        var smsRecipient = $('#smsRecipient').val();
        var dateTime = new Date();
        template.DateCreated = dateTime.toISOString();
        template.EmailBody = editorContent;
        template.email = editorContent;
        template.EmailRecipient = recipient;
        template.EmailSender = sender;
        template.EmailSubject = subject;
        template.SMSBody = smsBody;
        template.SMSRecipient = smsRecipient;

        $http.post('api/EmailTemplateDetails/', template).then(function (response) {
            $scope.rows = response.data;

            $http.get('api/EmailTemplateDetails').then(function (response) {
                $scope.rows = response.data;
                $scope.viewby = 5;
                $scope.totalItems = response.data.length;
                $scope.itemsPerPage = $scope.viewby;
                $scope.maxSize = 5;
                document.getElementById("pageCount").nodeValue = $scope.viewby;
                document.getElementById("alertNewTemplate").style.display = 'block';
            })



        });




    }

    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }

    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    //https://needlesapps.johnsonlawgroup.com/tools
    $scope.uploadFile = function () {


        var file = $scope.myFile;
        var uploadUrl = "MassMailer/LoadData/";

        var fd = new FormData();
        fd.append('file', file);
        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {

            $scope.recipientListRows = response.data.recipients;
            $scope.recpientViewBy = 10;
            $scope.recipientTotalItems = response.data.recipients.length;
            $scope.recipientCurrentPage = 1;
            $scope.recipientItemsPerPage = $scope.recpientViewBy;
            $scope.RecipientPages = 5
            //$scope.maxSize = 5;
            document.getElementById("pageCountRecipientList").nodeValue = $scope.viewByRecipient;

        });
    };









    $scope.recipientSetItemsPerPage = function (num) {
        if (num != 'All') {
            $scope.recipientItemsPerPage = num;
            $scope.recipientCurrentPage = 1; //reset to first page
        } else {
            $scope.recipientItemsPerPage = 999999
        }
    }

    $scope.submit = function () {
        if ($scope.form.file.$valid && $scope.file) {
            $scope.upload($scope.file);
        }
    };
}]);
var jlgMailerApp = angular.module('templateDatagrid', ['summernote', 'ui.bootstrap'])

jlgMailerApp.directive('fileModel', ['$parse', function ($parse) {
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

jlgMailerApp.controller('templateController', ['$scope', '$http',  function ($scope, $http) {


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

        }, function error(response) {
          
                alert('Error');

        });
      
    $http.get('MassMailer/GetFolderTemplatesList/').then(function (response) {

        $scope.FoldersTemplates = response.data.folders;
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


    $scope.loadVendorTemplates = function () {

     
        var folderId = $scope.selectedFolder.id;
        var folderName = $scope.selectedFolder.team_name

        var templateParameters = { 'FolderId': folderId, 'FolderName': folderName };
        $http.post('MassMailer/SelectedTemplateFolder/', templateParameters).then(function (response) {
            $scope.templateRows = response.data.templates;

        });
    };

    $scope.createDocument = function (templateRow) {

        var xlsxFileAndPath = $scope.excelFile;
        templateRow.caseNumber = $scope.casenum;
        templateRow.templateFolderId=$scope.selectedFolder.id
        templateRow.excelFilePath = xlsxFileAndPath;
        startUpdatingDocCreateProgressIndicator();
        $http.post('MassMailer/SignNowGenerateDocument/', templateRow).then(function (response) {


        });
        var intervalId;

        function startUpdatingDocCreateProgressIndicator() {
            $("#progress").show();

            intervalId = setInterval(
                function () {
                    // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                    $.post('MassMailer/Progress',function (progress) {

                            $("#progress").css({ width: progress + "%" });
                            $("#progress").html(progress + "%");
                        }
                    );
                },
                10
            );
        }

    }
    function stopUpdatingDocCreateProgressIndicator() {
        clearInterval(intervalId);
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


            template.SMSRecipient = smsRecipient;
      
            $http.post('api/EmailTemplateDetails/' , template).then(function (response) {
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

            $scope.recipientListRows = response.data.values.Recipients;          
            $scope.excelHeaderListRows = response.data.values.Recipients[0].Columns;

            $scope.recpientViewBy = 10;
            $scope.recipientTotalItems = response.data.values.Recipients.length;
            $scope.recipientCurrentPage = 1;
            $scope.recipientItemsPerPage = $scope.recpientViewBy;
            $scope.RecipientPages = 5
            //$scope.maxSize = 5;
            document.getElementById("pageCountRecipientList").nodeValue = $scope.viewByRecipient;
            document.getElementById("fileAndPath").value = response.data.values.FileAndPath;
           
            $scope.excelFile = response.data.values.FileAndPath;
           
            var excel = response.data.values.Recipients[0];
            var excelHeader = "";
            for (col of excel.Columns) {
                
                var excelHeader = excelHeader + '[[' + col.Column + ']] ';
                $scope.excelHeaders = excelHeader.trim();
            }
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


}]);
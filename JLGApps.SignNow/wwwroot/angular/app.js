angular.module('templateDatagrid', ['summernote', 'ui.bootstrap'])

    .controller('templateController', ['$scope', '$http', function ($scope, $http) {
       
        $http({
            method: 'GET',
            url: '/api/EmailTemplateDetails'

        }).then(function success(response) {

            $scope.rows = response.data;
            $scope.viewby = 5;
            $scope.totalItems = response.data.length;
            $scope.currentPage = 1;
            $scope.itemsPerPage = $scope.viewby;
            $scope.maxSize = 5;
            document.getElementById("pageCount").nodeValue = $scope.viewby;
        }, function error(response) {
        });
 

    $scope.delete = function (row) {
        var Id = row.TemplateId;       
        $http.delete(' api/EmailTemplateDetails/' + Id).then(function success(response) {

            $http({

                method: 'GET',

                url: '/api/EmailTemplateDetails'


            }).then(function success(response) {

                // this function will be called when the request is success
      
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
               
            }, function error(response) {

                // this function will be called when the request returned error status
            });

        }, function error(response) {

            // this function handles error

        });
    }
       
  
        $scope.templateLoader = function (row) {

            var templateId = row.TemplateId;

            $http({

                method: 'GET',

                url: '/api/EmailTemplateDetails/' + templateId


            }).then(function success(response) {
                $('#summernote').summernote('reset');
         
                $('#summernote').summernote('pasteHTML', response.data.EmailBody);
                document.getElementById('subject').value = response.data.EmailSubject;
                document.getElementById('from').value = response.data.EmailSender;
                document.getElementById('recipientEmail').value = response.data.EmailRecipient;
            
            }, function error(response) {


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

        $http.post('/api/EmailTemplateDetails/', template).then(function (response) {
            $scope.rows = response.data;         
          

            $http({
                method: 'GET',
                url: '/api/EmailTemplateDetails'
            }).then(function success(response) {
                $scope.rows = response.data;
                $scope.viewby = 5;
                $scope.totalItems = response.data.length;               
                $scope.itemsPerPage = $scope.viewby;
                $scope.maxSize = 5;
                document.getElementById("pageCount").nodeValue = $scope.viewby;
                document.getElementById("alertNewTemplate").style.display = 'block';
            }, function error(response) {


            });

        }, function (response) {

            // this function handles error

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
}]);
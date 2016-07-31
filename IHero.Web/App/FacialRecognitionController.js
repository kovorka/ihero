(function () {
    "use strict"
    angular.module("myApp").controller("FacialRecognitionController", FacialRecognitionController);
    angular.module("myApp").directive("fileModel", directive);

    
    function FacialRecognitionController($http, $stateParams) {
        var vm = this;
        vm.uploadFile = uploadFile;
        vm.uploadFileDeep = uploadFileDeep;

        function SetCanvasImageAndRectangle(elementId, photoUrl, photoWidth, photoHeight, recLeft, recTop, recWidth, recHeight, face) {
            var canvas = document.createElement('canvas');
            canvas.id = elementId;
            canvas.className += "pull-right";
            canvas.width = photoWidth;
            canvas.height = photoHeight;
            var context = canvas.getContext('2d');

            var imageObj = new Image();
            imageObj.onload = function () {
                //add image to canvas.
                context.drawImage(imageObj, 0, 0, photoWidth, photoHeight);

                //add face rectangle on top of the image.
                context.beginPath();
                context.rect(recLeft, recTop, recWidth, recHeight);
                context.lineWidth = 3;
                context.strokeStyle = 'yellow';
                context.stroke();
            }

            imageObj.src = photoUrl;

            var photoDiv = document.getElementById("photoDiv");
            photoDiv.appendChild(canvas);
            var br = document.createElement('br');

            var photoDetailsDiv = document.getElementById("photoDetailsDiv");

            var age = document.createElement('span');
            age.id = elementId + "-age";
            age.innerText = "Age: " + face.Age;
            photoDetailsDiv.appendChild(age);
            var br2 = document.createElement('br');
            photoDetailsDiv.appendChild(br2);

            var gender = document.createElement('span');
            gender.id = elementId + "-gender";
            gender.innerText = "Gender: " + face.Gender;
            photoDetailsDiv.appendChild(gender);
            var br3 = document.createElement('br');
            photoDetailsDiv.appendChild(br3);

            var glasses = document.createElement('span');
            glasses.id = elementId + "-glasses";
            glasses.innerText = "Glasses: " + face.Glasses;
            photoDetailsDiv.appendChild(glasses);
            var br4 = document.createElement('br');
            photoDetailsDiv.appendChild(br4);

            var smile = document.createElement('span');
            smile.id = elementId + "-smile";
            if (face.HasSmile) {
                smile.innerText = "Smiling: Yes";
            }
            else {
                smile.innerText = "Smiling: No";
            }
            photoDetailsDiv.appendChild(smile);
            var br5 = document.createElement('br');
            photoDetailsDiv.appendChild(br5);
        }//end SetCanvasImageAndRectangle

        function CreateCanvasImageAndRectangle(elementId, photoUrl, photoWidth, photoHeight, recLeft, recTop, recWidth, recHeight, face) {
            var results = document.getElementById("resultsDiv");

            var rowDiv = document.createElement('div');
            rowDiv.className += "row";
            results.appendChild(rowDiv);

            var photoRow = document.createElement('div');
            photoRow.className += "col-xs-6";
            rowDiv.appendChild(photoRow);

            var photoDetailsRow = document.createElement('div');
            photoDetailsRow.className += "col-xs-6";
            rowDiv.appendChild(photoDetailsRow);

            var canvas = document.createElement('canvas');
            canvas.className += "pull-right";
            canvas.id = elementId;
            canvas.width = photoWidth;
            canvas.height = photoHeight;
            var context = canvas.getContext('2d');

            var imageObj = new Image();
            imageObj.onload = function () {
                //add image to canvas.
                context.drawImage(imageObj, 0, 0, photoWidth, photoHeight);

                //add face rectangle on top of the image.
                context.beginPath();
                context.rect(recLeft, recTop, recWidth, recHeight);
                context.lineWidth = 3;
                context.strokeStyle = 'yellow';
                context.stroke();
            }

            imageObj.src = photoUrl;
            photoRow.appendChild(canvas);

            var match = document.createElement('h3');
            match.id = elementId + "-match";
            match.innerText = face.MatchConfidence + "% Match";
            photoDetailsRow.appendChild(match);
            var br6 = document.createElement('br');
            photoDetailsRow.appendChild(br6);

            var description = document.createElement('strong');
            description.id = elementId + "-description";
            description.innerText = face.Photo.Description;
            photoDetailsRow.appendChild(description);
            var br1 = document.createElement('br');
            photoDetailsRow.appendChild(br1);

            var age = document.createElement('span');
            age.id = elementId + "-age";
            age.innerText = "Age: " + face.Age;
            photoDetailsRow.appendChild(age);
            var br2 = document.createElement('br');
            photoDetailsRow.appendChild(br2);

            var gender = document.createElement('span');
            gender.id = elementId + "-gender";
            gender.innerText = "Gender: " + face.Gender;
            photoDetailsRow.appendChild(gender);
            var br3 = document.createElement('br');
            photoDetailsRow.appendChild(br3);

            var glasses = document.createElement('span');
            glasses.id = elementId + "-glasses";
            glasses.innerText = "Glasses: " + face.Glasses;
            photoDetailsRow.appendChild(glasses);
            var br4 = document.createElement('br');
            photoDetailsRow.appendChild(br4);

            var smile = document.createElement('span');
            smile.id = elementId + "-smile";
            if(face.HasSmile)
            {
                smile.innerText = "Smiling: Yes";
            }
            else
            {
                smile.innerText = "Smiling: No";
            }
            photoDetailsRow.appendChild(smile);
            var br5 = document.createElement('br');
            photoDetailsRow.appendChild(br5);

            var host = document.createElement('span');
            host.id = elementId + "-host";
            host.innerText = "Hosting Site: " + face.Photo.HostingSite;
            photoDetailsRow.appendChild(host);
            var br9 = document.createElement('br');
            photoDetailsRow.appendChild(br9);

            var war = document.createElement('span');
            war.id = elementId + "-match";
            var conflict = face.Photo.WarConflicts ? face.Photo.WarConflicts : 'Unknown';
            war.innerText = "War Conflict: " + conflict;
            photoDetailsRow.appendChild(war);
            var br8 = document.createElement('br');
            photoDetailsRow.appendChild(br8);

            var link = document.createElement('a');
            link.id = elementId + "-link";
            link.innerText = "More Details";
            link.setAttribute('href', face.Photo.ContentUrl);
            link.setAttribute('target', '_blank');
            photoDetailsRow.appendChild(link);
            var br7 = document.createElement('br');
            photoDetailsRow.appendChild(br7);

            var br10 = document.createElement('br');
            results.appendChild(br10);
            
            vm.loading = false;
        }//end SetCanvasImageAndRectangle

        function uploadFile() {
            $("#photoDiv").empty();
            $("#photoDetailsDiv").empty();
            $("#resultsDiv").empty();
            vm.loading = true;
            var file = vm.myFile;

            var reader = new FileReader();

            reader.addEventListener("load", function () {
                vm.imageSrc = reader.result;
            }, false);

            if (file) {
                reader.readAsDataURL(file);
            }

            console.log('file is ');
            console.dir(file);

            uploadFileToUrl(file);
        };

        function uploadFileDeep() {
            $("#photoDiv").empty();
            $("#photoDetailsDiv").empty();
            $("#resultsDiv").empty();
            vm.loading = true;
            var file = vm.myFile;

            var reader = new FileReader();

            reader.addEventListener("load", function () {
                vm.imageSrc = reader.result;
            }, false);

            if (file) {
                reader.readAsDataURL(file);
            }

            console.log('file is ');
            console.dir(file);

            uploadFileToUrlDeep(file);
        };

        function uploadFileToUrl(file) {
            var fd = new FormData();
            fd.append('file', file);

            $http.post("../api/FacialRecognition/UploadFile", fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })

            .then(function (response) {
                vm.FaceVM = response.data;
                SetCanvasImageAndRectangle('ScaledImageCanvas', vm.imageSrc, vm.FaceVM.SearchFaces[0].Photo.ScaledWidth, vm.FaceVM.SearchFaces[0].Photo.ScaledHeight, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Left, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Top, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Width, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Height, vm.FaceVM.SearchFaces[0]);

                for (var i = 0; i < vm.FaceVM.ResultFaces.length; i++) {
                    CreateCanvasImageAndRectangle(vm.FaceVM.ResultFaces[i].CortanaFaceId, vm.FaceVM.ResultFaces[i].Photo.PhotoUrl, vm.FaceVM.ResultFaces[i].Photo.ScaledWidth, vm.FaceVM.ResultFaces[i].Photo.ScaledHeight, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Left, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Top, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Width, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Height, vm.FaceVM.ResultFaces[i]);
                };

            })

            .then(function (response) {

            });
        };

        function uploadFileToUrlDeep(file) {
            var fd = new FormData();
            fd.append('file', file);

            $http.post("../api/FacialRecognition/UploadFileDeep", fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })

            .then(function (response) {
                vm.FaceVM = response.data;
                SetCanvasImageAndRectangle('ScaledImageCanvas', vm.imageSrc, vm.FaceVM.SearchFaces[0].Photo.ScaledWidth, vm.FaceVM.SearchFaces[0].Photo.ScaledHeight, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Left, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Top, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Width, vm.FaceVM.SearchFaces[0].FaceRectangleResized.Height, vm.FaceVM.SearchFaces[0]);

                for (var i = 0; i < vm.FaceVM.ResultFaces.length; i++) {
                    CreateCanvasImageAndRectangle(vm.FaceVM.ResultFaces[i].CortanaFaceId, vm.FaceVM.ResultFaces[i].Photo.PhotoUrl, vm.FaceVM.ResultFaces[i].Photo.ScaledWidth, vm.FaceVM.ResultFaces[i].Photo.ScaledHeight, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Left, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Top, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Width, vm.FaceVM.ResultFaces[i].FaceRectangleResized.Height, vm.FaceVM.ResultFaces[i]);
                };

            })

            .then(function (response) {

            });
        };

    }//end controller

    function directive ($parse) {
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
    }
}//end function
());
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
//Change Information
document.addEventListener("DOMContentLoaded", function() {
  var informationLink = document.querySelector("a[href='#changepass']");
  var changepassLink = document.querySelector("a[href='#information']");
  var transactionLink = document.querySelector("a[href='#transaction']");
  var informationSection = document.getElementById("information");
  var changepassSection = document.getElementById("changepass");
  var transactionSection = document.getElementById("transaction");

  // Kiểm tra nếu URL chứa '#information', '#changepass', '#myCourse' hoặc '#transaction'
  if (window.location.href.includes('#information')) {
      informationLink.classList.add("text-primary");
      changepassSection.style.display = "none";
      transactionSection.style.display = "none";
  } else if (window.location.href.includes('#changepass')) {
      changepassLink.classList.add("text-primary");
      informationSection.style.display = "none";
      transactionSection.style.display = "none";
  } else if (window.location.href.includes('#transaction')) {
      transactionLink.classList.add("text-primary");
      informationSection.style.display = "none";
      changepassSection.style.display = "none";
  } else {
      informationLink.classList.add("text-primary");
      changepassSection.style.display = "none";
      transactionSection.style.display = "none";
  }

  informationLink.addEventListener("click", function(e) {
      e.preventDefault();
      informationSection.style.display = "block";
      informationLink.classList.add("text-primary");
      changepassSection.style.display = "none";
      changepassLink.classList.remove("text-primary");
      transactionSection.style.display = "none";
      transactionLink.classList.remove("text-primary");
  });

  changepassLink.addEventListener("click", function(e) {
      e.preventDefault();
      changepassSection.style.display = "block";
      changepassLink.classList.add("text-primary");
      informationSection.style.display = "none";
      informationLink.classList.remove("text-primary");
      transactionSection.style.display = "none";
      transactionLink.classList.remove("text-primary");
  });

  transactionLink.addEventListener("click", function(e) {
      e.preventDefault();
      transactionSection.style.display = "block";
      transactionLink.classList.add("text-primary");
      informationSection.style.display = "none";
      informationLink.classList.remove("text-primary");
      changepassSection.style.display = "none";
      changepassLink.classList.remove("text-primary");
  });
});


document.addEventListener("DOMContentLoaded", function() {
  var informationLink = document.querySelector("a[href='#changepass']");
  var changepassLink = document.querySelector("a[href='#information']");
  var informationSection = document.getElementById("information");
  var changepassSection = document.getElementById("changepass");

  // Kiểm tra nếu URL chứa '#information', '#changepass', '#myCourse' hoặc '#transaction'
  if (window.location.href.includes('#information')) {
      informationLink.classList.add("text-primary");
      changepassSection.style.display = "none";
  } else if (window.location.href.includes('#changepass')) {
      changepassLink.classList.add("text-primary");
      informationSection.style.display = "none";
  } else {
      informationLink.classList.add("text-primary");
      changepassSection.style.display = "none";
  }

  informationLink.addEventListener("click", function(e) {
      e.preventDefault();
      informationSection.style.display = "block";
      informationLink.classList.add("text-primary");
      changepassSection.style.display = "none";
      changepassLink.classList.remove("text-primary");
  });

  changepassLink.addEventListener("click", function(e) {
      e.preventDefault();
      changepassSection.style.display = "block";
      changepassLink.classList.add("text-primary");
      informationSection.style.display = "none";
      informationLink.classList.remove("text-primary");
  });
});

//Time of video
var video = document.getElementById('myVideo');
video.addEventListener('loadedmetadata', function() {
    var duration = Math.round(video.duration);
    var minutes = Math.floor(duration / 60);
    var seconds = duration % 60;
    var formattedDuration = minutes + ':' + (seconds < 10 ? '0' : '') + seconds;
    document.getElementById('videoDuration').innerText = 'Thời lượng: ' + formattedDuration;
});

// Write your JavaScript code.
function time() {
    var today = new Date();
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";
    var day = weekday[today.getDay()];
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    m = checkTime(m);
    s = checkTime(s);
    nowTime = h + "h " + m + "m " + s + "s";
    if (dd < 10) {
      dd = '0' + dd
    }
    if (mm < 10) {
      mm = '0' + mm
    }
    today = day + ', ' + dd + '/' + mm + '/' + yyyy;
    tmp = '<span class="date"> ' + today + ' - ' + nowTime +
      '</span>';
    document.getElementById("clock").innerHTML = tmp;
    clocktime = setTimeout("time()", "1000", "Javascript");
  
    function checkTime(i) {
      if (i < 10) {
        i = "0" + i;
      }
      return i;
    }
  }

// Mode
document.querySelector(".theme-toggle").addEventListener("click", () => {
  toggleLocalStorage();
  toggleRootClass();
});

function toggleRootClass(){
  const current = document.documentElement.getAttribute('data-bs-theme');
  const inverted = current == 'dark' ? 'light' : 'dark';
  document.documentElement.setAttribute('data-bs-theme', inverted);
};

function toggleLocalStorage(){
  if(isLight()){
    localStorage.removeItem("light");
  }else{
    localStorage.setItem("light", "set");
  }
};

function isLight(){
  return localStorage.getItem("light");
};

if(isLight()){
  toggleRootClass();
}

//Alert
var editAlert = document.getElementById('editAlert');
var deleteAlert = document.getElementById('deleteAlert');
var progressBar = document.getElementById('progressBar');

if (editAlert && progressBar) {
    progressBar.addEventListener('animationend', function() {
        editAlert.style.display = 'none';
    });
}

if (deleteAlert && progressBar) {
    progressBar.addEventListener('animationend', function() {
        deleteAlert.style.display = 'none';
    });
}

//Active
// document.addEventListener('DOMContentLoaded', function() {
//   var navItems = document.querySelectorAll('.nav-item');
//   var defaultActiveItem = document.querySelector('.nav-item.active');

//   function setActiveItem() {
//       var activeItemIndex = localStorage.getItem('activeItemIndex');
//       if (activeItemIndex !== null) {
//           navItems.forEach(function(item, index) {
//               if (index.toString() === activeItemIndex) {
//                   item.classList.add('active');
//               } else {
//                   item.classList.remove('active');
//               }
//           });
//       } else {
//           defaultActiveItem.classList.remove('active');
//       }
//   }

//   function handleClick(event) {
//       var clickedItem = event.currentTarget;
//       var clickedItemIndex = Array.from(navItems).indexOf(clickedItem);
//       localStorage.setItem('activeItemIndex', clickedItemIndex.toString());
//   }

//   navItems.forEach(function(navItem) {
//       navItem.addEventListener('click', handleClick);
//   });

//   setActiveItem();
// });

//Check all box
document.addEventListener('DOMContentLoaded', function() {
  var allCheckbox = document.getElementById('all');
  var checkboxes = document.querySelectorAll('tbody input[type="checkbox"]');

  function toggleCheckboxes() {
      checkboxes.forEach(function(checkbox) {
          checkbox.checked = allCheckbox.checked;
      });
  }

  allCheckbox.addEventListener('click', toggleCheckboxes);
});

// Show more
function toggleButtonText(button) {
  if (button.innerText === "See more") {
      button.innerText = "See less";
  } else {
      button.innerText = "See more";
  }
}

// Information
function showContent(contentId) {
  // Ẩn tất cả các nội dung
  hideAllContent();

  // Hiển thị nội dung tương ứng với contentId
  var content = document.getElementById(contentId);
  content.style.display = "block";
}

function hideAllContent() {
  // Ẩn tất cả các nội dung
  var contents = document.getElementsByClassName("content");
  for (var i = 0; i < contents.length; i++) {
      contents[i].style.display = "none";
  }
}

//Đánh sao
function rateStar(rating) {
  document.getElementById("rating").value = rating;
  // Hãy thêm các xử lý tương ứng khác nếu cần
}

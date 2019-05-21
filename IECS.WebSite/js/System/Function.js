/*  */
function Responsive($a) {
    if ($a == true) $("#Device").css("opacity", "100");
    if ($a == false) $("#Device").css("opacity", "0");
    $('#iframe-wrap').removeClass().addClass('full-width');
    $('.icon-tablet,.icon-mobile-1,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
    $(this).addClass('active');
    return false;
}

function animateButton(e) {

    e.preventDefault;
    //reset animation
    e.target.classList.remove('animate');

    e.target.classList.add('animate');
    setTimeout(function () {
        e.target.classList.remove('animate');
    }, 700);

    setTimeout(function () {
        $(".bubbly-button").fadeOut(400);
        $(".transition-loader-inner").show();
    }, 100);
}
function RandomNum(min, max) {
    return Math.floor(min + Math.random() * (max - min));
}  
function getRandomString(len) {
    len = len || 32;
    var $chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678'; // 默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1  
    var maxPos = $chars.length;
    var pwd = '';
    for (i = 0; i < len; i++) {
        pwd += $chars.charAt(Math.floor(Math.random() * maxPos));
    }
    return pwd;
}  

function SetPre(str, id) {
    console.log(str + " " + id);


    var index = 0;
    var IntervalId = 0;
    setTimeout(function () {
        var r = RandomNum(10, 200);
        IntervalId = setInterval(function () {
            $("#"+id).text(str.substring(0, index++));
            if (index > str.length) {
                clearInterval(IntervalId);
            }
            //console.log(index + " " + str.length);
        }, r);
    }, (r * str.length));


}
/*正确使用：异步处理逻辑回调一次显示一条  此处仅显示*/
$.LoadCommandLine = function (arr,callback) {
    if (arr.length > 0) {

        //处理逻辑


        //显示
        var id = getRandomString(5);
        $("#pre").append('<pre id="' + id + '" class="pre"></pre>');
        var index = 0;
        var IntervalId = 0;
        var r = RandomNum(10, 200);
        IntervalId = setInterval(function () {

            $("#" + id).text(arr[0].substring(0, index++));
            if (index > arr[0].length) {
                clearInterval(IntervalId);
            }
            //console.log(index + " " + str.length);
        }, r);

        //下一个
        setTimeout(function () {
            $.LoadCommandLine(arr.slice(1), callback);
        }, (r * arr[0].length));

        if ($(".pre").length > 5) $("#pre").find("pre:first-child").remove();

    }else{ callback(); }
}
    

$.LoadContextMenu = function (arr) {

    var _arr = $.map(arr, function (text,id) {
        $("#menu").append('<div class="menu" id="' + id + '">' + text + '</div>');
        return true;
    });

    _arr.push($.Deferred(function (deferred) {
        $(deferred.resolve);
    }));

    return $.when.apply($, _arr);
}


function X() {
    Source = document.body.firstChild.data;
    document.close();
    document.open();
    document.body.innerHTML = Source;
}

function fullScreen() {
	  var el = document.documentElement;
	  var rfs = el.requestFullScreen || el.webkitRequestFullScreen;
	  if(typeof rfs != "undefined" && rfs) {
	    rfs.call(el);
	  } else if(typeof window.ActiveXObject != "undefined") {
	    var wscript = new ActiveXObject("WScript.Shell");
	    if(wscript != null) {
	        wscript.SendKeys("{F11}");
	    }
	}else if (el.msRequestFullscreen) {
          el.msRequestFullscreen();
	}else if(el.oRequestFullscreen){	
          el.oRequestFullscreen();
    }else{ 	
    	swal({   title: "浏览器不支持全屏调用！",   text: "请更换浏览器或按F11键切换全屏！(3秒后自动关闭)", type: "error",  timer: 3000 });	       
    }
}

function closeWindows() {
    var browserName = navigator.appName;
    var browserVer = parseInt(navigator.appVersion);
    //alert(browserName + " : "+browserVer);
 
    //document.getElementById("flashContent").innerHTML = "<br>&nbsp;<font face='Arial' color='blue' size='2'><b> You have been logged out of the Game. Please Close Your Browser Window.</b></font>";
 
    if(browserName == "Microsoft Internet Explorer"){
        var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;  
        if (ie7)
        {  
            //This method is required to close a window without any prompt for IE7 & greater versions.
            window.open('','_parent','');
            window.close();
        }
        else
        {
            //This method is required to close a window without any prompt for IE6
            this.focus();
            self.opener = this;
            self.close();
        }
    }else{  
        //For NON-IE Browsers except Firefox which doesnt support Auto Close
        try{
            this.focus();
            self.opener = this;
            self.close();
        }
        catch(e){
 
        }
 
        try{
            window.open('','_self','');
            window.close();
        }
        catch(e){
 
        }
    }
}

//console.log("Loaded Function");
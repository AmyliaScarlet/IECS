window.oncontextmenu = function (e) {
    //取消默认的浏览器自带右键
    e.preventDefault();

    if ($("#menu").find("div").length<=0)
    {
        $.LoadContextMenu({
            "menu_sub1": "刷 新",
            "menu_sub2": "退 出",
            "menu_sub3": "全 屏",
        }).done(function () {
            OnclickListener();
        });
    }

    //获取我们自定义的右键菜单
    var menu = document.querySelector("#menu");
    if (menu != null) {
        //根据事件对象中鼠标点击的位置，进行定位
        menu.style.left = e.clientX + 'px';
        menu.style.top = e.clientY + 'px';

        if (e.clientX > $(window).width() - $("#menu").width()) {
            menu.style.left = ($(window).width() - $("#menu").width()) + 'px';
        }
        if ($("#menu").height() == 0) $("#menu").height(25 * $("#menu").children().length);
        if (e.clientY > $(window).height() - $("#menu").height()) {
            menu.style.top = ($(window).height() - $("#menu").height()) + 'px';
        }
        //console.log(e.clientY + " " + $(window).height() + " " + $("#menu").height());
        //改变自定义菜单的宽，让它显示出来
        showMenu();
    } else {
        alert(1);
    }

}

//关闭右键菜单，很简单
window.onclick = function (e) {
    //用户触发click事件就可以关闭了，因为绑定在window上，按事件冒泡处理，不会影响菜单的功能
    hideMenu();
}

function showMenu()
{
    if ($("#menu").css("display") != 'none') {
        $("#menu").hide();
        $("#menu").show(200);
    }
    else
    {
        $("#menu").show(200);
    }

    swal('更新成功', '', 'success');   
}

function hideMenu() {
    $("#menu").hide(200);

    $("#menu").find("div").remove();
}


function OnclickListener() {
    $("#menu_sub1").click(function (e) {
        window.localStorage.clear();
        location.reload();
    });
    $("#menu_sub2").click(function (e) {
        if (confirm("您确定要退出吗？")) {
            window.localStorage.clear();
            closeWindows();
        }
    });
    $("#menu_sub3").click(function (e) {
        fullScreen();
    });
}

//console.log("Loaded oncontextmenu");
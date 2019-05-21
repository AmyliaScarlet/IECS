IsDebug = true;
function Log(msg) {
    if (IsDebug) console.log(msg);
}
function isInclude(name) {
    var js= /js$/i.test(name);
    var es=document.getElementsByTagName(js?'script':'link');
    for(var i=0;i<es.length;i++) 
    if(es[i][js?'src':'href'].indexOf(name)!=-1)return true;
    return false;
}
function nodeToString(node) {
    var tmpNode = document.createElement("div");
    tmpNode.appendChild(node.cloneNode(true));
    var str = tmpNode.innerHTML;
    tmpNode = node = null; 
    return str;
}
function XPathMatch(STR_XPATH,docum) {
    var xresult = document.evaluate(STR_XPATH, docum, null, XPathResult.ANY_TYPE, null);
    var xnodes = [];
    var xres;
    while (xres = xresult.iterateNext()) {
        xnodes.push(xres);
    }
    var str = '';
    for (i = 0; i < xnodes.length;i++)
    {
        str += nodeToString(xnodes[i]);
    }
    return str;
}

function Kernel() {
    $("body").hide(); $("body").height(window.screen.height);



    if (!isInclude("jquery-1.8.3.min.js")) return alert("Init Error: jQuery Not Found ");



    $.BrowserCheck = function () {
        setTimeout(function () {
            $.extend({
                NV: function (name) {
                    var NV = {};
                    var UA = navigator.userAgent.toLowerCase();
                    try {
                        NV.name = !-[1,] ? 'ie' :
                            (UA.indexOf("firefox") > 0) ? 'firefox' :
                                (UA.indexOf("chrome") > 0) ? 'chrome' :
                                    window.opera ? 'opera' :
                                        window.openDatabase ? 'safari' :
                                            'unkonw';
                    } catch (e) { };
                    try {
                        NV.version = (NV.name == 'ie') ? UA.match(/mozilla ([\d.]+)/)[1] :
                            (NV.name == 'firefox') ? UA.match(/firefox\/([\d.]+)/)[1] :
                                (NV.name == 'chrome') ? UA.match(/chrome\/([\d.]+)/)[1] :
                                    (NV.name == 'opera') ? UA.match(/opera.([\d.]+)/)[1] :
                                        (NV.name == 'safari') ? UA.match(/version\/([\d.]+)/)[1] :
                                            '0';
                    } catch (e) { };
                    try {
                        NV.shell = (UA.indexOf('360ee') > -1) ? '360极速浏览器' :
                            (UA.indexOf('360se') > -1) ? '360安全浏览器' :
                                (UA.indexOf('se') > -1) ? '搜狗浏览器' :
                                    (UA.indexOf('aoyou') > -1) ? '遨游浏览器' :
                                        (UA.indexOf('theworld') > -1) ? '世界之窗浏览器' :
                                            (UA.indexOf('worldchrome') > -1) ? '世界之窗极速浏览器' :
                                                (UA.indexOf('greenbrowser') > -1) ? '绿色浏览器' :
                                                    (UA.indexOf('qqbrowser') > -1) ? 'QQ浏览器' :
                                                        (UA.indexOf('baidu') > -1) ? '百度浏览器' :
                                                            'Unknown or None';
                    } catch (e) { }
                    switch (name) {
                        case 'UA': br = UA; break;
                        case 'name': br = NV.name; break;
                        case 'version': br = NV.version; break;
                        case 'shell': br = NV.shell; break;
                        default: br = NV.name;
                    }

                    return br;
                }
            });
            Log('BrowserUA=' + $.NV('UA') +
                '\nBrowserName=' + $.NV('name') +
                '\nBrowserVersion=' + parseInt($.NV('version')) +
                '\nBrowserShell=' + $.NV('shell'));
            if ($.NV('name') != 'chromexxx') {
                setTimeout(function () {
                    swal({
                        title: "自动关闭弹窗！",
                        text: "2秒后自动关闭。",
                        timer: 4000
                    });
                }, 1000); return false;
            }
            if ($.browser.msie && (parseInt($.browser.version) < 9)) {
                document.writeln("你的浏览器版本太低了,请升级您的浏览器。推荐使用：谷歌或其他双核极速模式。如果您的使用的是360、搜狗、QQ等双核浏览器，请切换到极速模式访问！");
                document.execCommand("Stop");
            }
        }, 1);
    }

    //定义
    $.getMultiScripts = function (arr) {
        var _arr = $.map(arr, function (scr) {
            $.getScript(scr);
            return true;
        });

        _arr.push($.Deferred(function (deferred) {
            $(deferred.resolve);
        }));

        return $.when.apply($, _arr);
    }

    var SetMainCssMode = {
        ADD: 1,
        DELETE: 2
    }

    $.getMultiCss = function (arr,pageName) {

        var _arr = $.map(arr, function (scr) {
            if (scr == "css/Main.css") {
                //动态设定Main.css
                $.SetMainCss(pageName, SetMainCssMode.ADD);
            }

            $("head").append("<link>");
            var css = $("head").children(":last");
            css.attr({
                rel: "stylesheet",
                type: "text/css",
                href: scr
            });
            return true;
        });

        _arr.push($.Deferred(function (deferred) {
            $(deferred.resolve);
        }));

        return $.when.apply($, _arr);
    }

    $.getFrontLib = function (jPageName) {
        $.ajaxSettings.async = false;
        $.getJSON("FrontLib.json", function (data) {
            var jLib = data[jPageName];

            if (String(jLib["css"]).length > 0) {
                //要加载的css
                $.getMultiCss(jLib["css"], jPageName).done(function () {
                    Log(jPageName + " all css loaded");
                });
            }

            //动态加载对应页面的js
            jLib["js"].push('js/PageJs/' + jPageName + '.js');

            if (String(jLib["js"]).length > 0) {
                //要加载的js
                $.getMultiScripts(jLib["js"]).done(function () {
                    Log(jPageName + " all scripts loaded");
                });
            }

        })
        $.ajaxSettings.async = true;

    }

    $.LoadBody = function (pagePath) {
        pagePath = 'Page/' + pagePath + '.html';
        $("body").html("");
        var storage = window.localStorage;
        var pageContent = storage.getItem(pagePath);
        
        if (pageContent != null) {
            Log(pagePath+" has pageContent");
            $("body").append(pageContent);
        }
        else {
            Log(pagePath+" none pageContent");
            //setTimeout(function () {
            $.get(pagePath, function (pageContent) {
                var parser = new DOMParser();
                var xmlDoc = parser.parseFromString(pageContent, "text/xml");
                pageContent = String(XPathMatch('//body/*', xmlDoc));
                var storage = window.localStorage;
                storage.setItem(pagePath, pageContent);
                $("body").append(pageContent);
            });
            //}, 1);
        }
    }

    $.SetMainCss = function (jPageName,mode) {

        var pData = {
            "pageName": jPageName,
            "mode": mode
        }
        $.ajax({
            async: false,
            type: 'POST',
            url: 'CssHandler.ashx',
            data: JSON.stringify(pData),
            dataType: 'json',
            success: function (res) {
                if (res == "0") Log(jPageName + " " + mode + " Set Fail!");
            }, error: function () {
                Log(jPageName + " " + mode + " Post Fail!");
            }
        });

    }

    jNow = '';
    jIndex = 0;
    jHistory = [];
    $.PushHistory = function (sPageName) {
        setTimeout(function () {
            if (!!(window.history && history.pushState)) {

                if (sPageName.toLowerCase() == 'default') {
                    window.history.replaceState({ "pageName": sPageName }, "");
                    Log("replaceState: " + sPageName);
                } else {
                    var has = false;
                    for (var i = 0; i < jHistory.length; i++) {
                        if (jHistory[i] == sPageName) {
                            has = true;
                        }
                    }
                    if (!has) {
                        jHistory.push(sPageName);
                        window.history.pushState({ "pageName": sPageName }, "");
                        Log("pushState: " + sPageName);
                    }
                }
                jNow = sPageName;

                Log("history ：\nlength: " + window.history.length + "\n" + jHistory);
            } else {
                Log("UnSupport history!");
            }
        }, 1);
    }

    $.LoadPage = function (sPageName)
    {
        //开始加载页面
        $.LoadBody(sPageName);

        //加载CSS JS
        $.getFrontLib(sPageName);

        //还原Main.css
        $.SetMainCss(sPageName, SetMainCssMode.DELETE);

        //历史记录入栈
        $.PushHistory(sPageName);

    }

    //禁用F5
    $(document).bind("keydown", function (e) {
        e = window.event || e;
        if (e.keyCode == 116) {
            e.keyCode = 0;
            return false;
        }
    }); 

    var strUrl = window.location.href;
    var arrUrl = strUrl.split("/");
    var strPage = arrUrl[arrUrl.length - 1];
    var strPageName = strPage.split(".")[0];

    window.addEventListener('popstate', function (event){
        var toPageName = event.state["pageName"]
        Log('state: ' + toPageName);
        Log(jNow + '  ' + toPageName);

        if (jNow != toPageName) $.LoadPage(toPageName); 
    }, false);


    //加载页面
    $.LoadPage(strPageName);

    //检查浏览器
    $.BrowserCheck();

    //显示页面
    $("body").fadeIn(200);
}
Kernel();
//$(function () {  

//})



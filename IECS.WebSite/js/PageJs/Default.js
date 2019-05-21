/*** Default.aspx页面初始化 ***/

//初始化控件
$(".bubbly-button").click(function (e) {
    animateButton(e);

    //注册sw
    RegisterServiceWorker();//$.RegisterServiceWorker();

    $.LoadCommandLine([
        '正在初始化...',
        '加载系统核心...',
        '加载FastCGI模块...',
        '加载脚本...',
        '开启连接通道...',
        '第一次接触...   成功',
        '启动后台服务...',
        '检测配置文件...',
        '检测SSH状态...',
        '第二次接触...   成功',
        '加载扩展模块...',
        '初始化HUB...',
        '系统启动中...',
        '系统启动完毕...',
        '第三次接触...   成功',
        '欢迎使用！',

    ], function () {
        setTimeout(function () {
            $.LoadPage("Home");
        }, 300);
    });

    return false;
});

function RegisterServiceWorker() {
    if ('serviceWorker' in navigator) {
        const sw = window.navigator.serviceWorker
        const killSW = window.killSW || false

        if (!sw) {
            return
        }

        if (!!killSW) {
            sw.getRegistration('/serviceWorker.js').then(registration => {
                // 手动注销
                registration.unregister()
            })
        } else {
            // 表示该 sw 监听的是根域名下的请求
            sw.register('/serviceWorker.js').then(registration => {
                // 注册成功后会进入回调
                Log("ServiceWorker registration scope:" + registration.scope)
            }).catch(err => {
                Log("err:" + err)
            })
        }
    } else {

        Log('ServiceWorker not support this explorer ');
    }
}
(function (signalR) {
    "use strict";

    var clientTracer = {};
    var method_startSpan = "StartSpan";
    var method_log = "Log";
    var method_setTags = "SetTags";
    var method_finishSpan = "FinishSpan";
    var methods = [method_startSpan, method_log, method_setTags, method_finishSpan];

    var connection = new signalR.HubConnectionBuilder().withUrl("/Hubs/TraceHub").build();
    
    function createServerFunction(methodName) {
        var serverFunc = function (args) {
            console.log("invoking server method " + methodName);
            console.log(args);
            connection.invoke(methodName, args).catch(function (err) {
                console.log("invoking server method err: " + methodName);
                return console.error(err.toString());
            });
        };
        return serverFunc;
    }
    
    function createDefaultCallback(methodName) {
        var callbackFunc = function (result) {
            console.log("invoking default client callback " + methodName);
            console.log(result);
        };
        return callbackFunc;
    }

    function initServerFunctions(tracer) {
        for (var i = 0; i < methods.length; i++) {
            var method = methods[i];
            tracer[method] = createServerFunction(method);
        }
        return tracer;
    }

    function initCallbacks(config) {
        for (var i = 0; i < methods.length; i++) {
            var method = methods[i];
            var callbackName = method + "Callback";

            if (config[callbackName] && typeof config[callbackName] === "function") {
                connection.on(callbackName, config[callbackName]);
            } else {
                connection.on(callbackName, createDefaultCallback(callbackName));
            }
        }
    }

    function init(config) {

        initCallbacks(config);

        connection.start().then(function () {
            if (config.connectionStarted && typeof config.connectionStarted === "function") {
                config.connectionStarted();
            } else {
                console.log("connectionStarted");
            }
        }).catch(function (err) {
            if (config.connectionError && typeof config.connectionError === "function") {
                config.connectionError(err);
            } else {
                console.error(err.toString());
            }
        });
    }

    clientTracer = initServerFunctions(clientTracer);
    
    clientTracer.init = init;

    document["clientTracer"] = clientTracer;
    return clientTracer;

}(signalR));
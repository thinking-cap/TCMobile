
console.log('loaded api');
// window.parent = window;
window.opener = window;
if (typeof ($) !== "undefined") {
        $.fn.height = function () {
            if (this[0].constructor.name.toLowerCase() == 'window') {
                return $(document).innerHeight();
            } else { return $(this).outerHeight(); }
    };

        $.ajaxSetup({
            dataType: "xml"
        });
    }

    

    var meta = document.createElement('meta');
    meta.setAttribute('name', 'viewport');
    meta.setAttribute('content', 'width=device-width');
    meta.setAttribute('initial-scale', '1.0');
    meta.setAttribute('maximum-scale', '1.0');
    meta.setAttribute('minimum-scale', '1.0');
    meta.setAttribute('user-scalable', 'no');
    try {
        document.getElementsByTagName('head')[0].appendChild(meta);
} catch (e) { }
if (typeof (API_1484_11) == 'undefined') {
    var config = {
        init: false,
        error: 0,
        api_return_bool: false,
        storeinteractions: true,
        dirtyCommit : null
    };

    var API_1484_11 = {
        Initialize: function () {
            console.log('initialized')
            config.init = true;
            var msg = {

                status: 'Initialize',
                cmi: JSON.stringify(cmi)

            }
            console.log("****************");
            console.log(cmi);
            console.log("****************");
            if (typeof (jsBridge) !== 'undefined') {
                jsBridge.invokeAction(JSON.stringify(msg));
            } else {
                window.webkit.messageHandlers.invokeAction.postMessage(JSON.stringify(msg));
            }
            if (typeof (cmi.interactions) !== 'undefined')
                cmi.interactions._count = (typeof (cmi.interactions) !== 'undefined') ? cmi.interactions.length : 0;
            else
                cmi.interactions = [];
              
            if (typeof (cmi.objectives) !== 'undefined')
                cmi.objectives._count = (typeof (cmi.objectives) !== 'undefined') ? cmi.objectives.length : 0;
            else
                cmi.objectives = [];
            return "true";
        },
        GetValue: function (key) {
            //if (config.init === true) {
                var val = "";
                if (key.indexOf("cmi.interactions") > -1) {
                    val = GetCMIInteractions(key);
                } else if (key.indexOf("cmi.objectives") > -1) {
                    val = GetCMIObjectives(key);
                } else if (key.indexOf("cmi.comments") > -1) {
                    val = GetCMIComments(key);
                } else {
                    if (key.indexOf('adl.nav.request_valid.choice') >= 0) {

                    } else {
                        config.error = 0;
                        switch (key) {
                            case "cmi._children": return cmi._children; break;
                            case "cmi.learner_name": val = cmi.learner_name; break;
                            case "cmi.learner_id": val = cmi.learner_id; break;
                            case "cmi.course_id": val = courseID; break;
                            case "cmi.location": val = cmi.location; break;
                            case "cmi.completion_status": val = cmi.completion_status; break;
                            case "cmi.success_status": val = cmi.success_status; break;
                            case "cmi.score.scaled": val = cmi.score.scaled; break;
                            case "cmi.entry": val = cmi.entry; break;
                            case "cmi.mode": val = cmi.mode; break;
                            case "cmi.score_children": val = cmi.score_children; break;
                            case "cmi.score.raw": val = cmi.score.raw; break;
                            case "cmi.score.min": val = cmi.score.min; break;
                            case "cmi.score.max": val = cmi.score.max; break;
                            case "cmi.total_time": val = cmi.total_time; break;
                            case "cmi.lesson_mode": val = cmi.lesson_mode; break;
                            case "cmi.suspend_data": val = cmi.suspend_data; break;
                            case "cmi.launch_data": val = cmi.launch_data; break;
                            case "cmi.comments": val = cmi.comments; break;
                            case "cmi.progress_measure": val = cmi.progress_measure; break;
                            case "cmi.comments_from_lms": val = cmi.comments_from_lms; break;
                            case "cmi.objectives._children": val = "id,score,success_status,completion_status,description"; break;
                            case "cmi.objectives._count": val = cmi.objectives.length; break;
                            case "cmi.student_data._children": val = cmi.student_data._children; break;
                            case "cmi.student_data.mastery_score": val = cmi.student_data.mastery_score; break;
                            case "cmi.student_data.max_time_allowed": val = cmi.student_data.max_time_allowed; break;
                            case "cmi.student_data.time_limit_action": val = cmi.student_data.time_limit_action; break;
                            case "cmi.learner_preference._children": val = cmi.learner_preference._children; break;
                            case "cmi.student_preference.audio": val = cmi.learner_preference.audio; break;
                            case "cmi.learner_preference.language": val = cmi.learner_preference.language; break;
                            case "cmi.learner_preference.speed": val = cmi.learner_preference.speed; break;
                            case "cmi.learner_preference.text": val = cmi.learner_preference.text; break;
                            case "adl.nav.request_valid.continue": val = adl.nav.request_valid["continue"]; break;
                            case "adl.nav.request_valid.previous": val = adl.nav.request_valid.previous; break;
                            case "adl.nav.request_valid.choice": val = adl.nav.request_valid.choice; break;
                            default: val = false; config.error = 401; break;
                        }
                    }
                }
                //if(config.committing !== true)
                // config.pingAPI();
                if (config.api_return_bool)
                    return val;
                else
                    return val.toString();
            //} else {
            //    config.error = (config.init === true) ? 0 : 301;
            //    if (config.api_return_bool)
            //        return false;
            //    else
            //        return 'false';
            //}
        },
        SetValue: function (key, value) {
            if (config.init === true) {
                if (config.dirtyCommit !== null) {
                    clearTimeout(config.dirtyCommit);
                    config.dirtyCommit = null;
                }
                var val = "";
                if (key.indexOf("cmi.interactions") > -1) {
                    if (config.storeinteractions === true) {
                        val = SetCMIInteractions(key, value);
                    } else {
                        val = true;
                    }
                } else if (key.indexOf("cmi.objectives") > -1) {
                    val = SetCMIObjectives(key, value);
                } else if (key.indexOf("cmi.comments") > -1) {
                    SetCMIComments(key, value);

                } else {
                    config.error = 0;
                    switch (key) {
                        case "cmi._children": config.error = 403; val = false; break;
                        case "cmi.student_name": config.error = 403; val = false; break;
                        case "cmi.student_id": config.error = 403; val = false; break;
                        case "cmi.score.scaled": config.error = 0; cmi.score.scaled = value; val = true; break;
                        case "cmi.location": val = CheckString('cmi.location', value); break;
                        case "cmi.completion_status": val = SetStatus(value); break;
                        case "cmi.success_status": config.error = 0; cmi.success_status = value; val = true; break;
                        case "cmi.entry": this.Error = 403; val = false; break;
                        case "cmi.score_children": config.error = 403; val = false; break;
                        case "cmi.exit": val = SetExit(value); break;
                        case "cmi.session_time": cmi.session_time = value; val = true; break; // val = SetSessionTime(value);
                        case "cmi.score.raw": val = true; cmi.score.raw = value; break;
                        case "cmi.score.min": val = true; cmi.score.min = value; break;
                        case "cmi.score.max": val = true; cmi.score.max = value; break;
                        case "cmi.progress_measure": val = true; cmi.progress_measure = value; break;
                        case "cmi.total_time": cmi.total_time = value; val = true; break;
                        case "cmi.lesson_mode": val = true; cmi.lesson_mode = value; break;
                        case "cmi.suspend_data": cmi.suspend_data = value; val = true; break;
                        case "cmi.launch_data": val = cmi.launch_data; break;
                        case "cmi.comments": val = cmi.comments; break;
                        case "cmi.comments_from_lms": val = cmi.comments_from_lms; break;
                        case "cmi.objectives._children": val = ["id", "score", "status"]; break;
                        case "cmi.objectives._count": val = cmi.objectives.length; break;
                        case "cmi.student_data._children": val = cmi.student_data._children; break;
                        case "cmi.student_data.mastery_score": val = cmi.student_data.mastery_score; break;
                        case "cmi.student_data.max_time_allowed": val = cmi.student_data.max_time_allowed; break;
                        case "cmi.student_data.time_limit_action": val = cmi.student_data.time_limit_action; break;
                        case "cmi.student_preference._children": val = cmi.student_preference._children; break;
                        case "cmi.student_preference.audio": val = cmi.student_preference.audio; break;
                        case "cmi.student_preference.language": val = cmi.student_preference.language; break;
                        case "cmi.student_preference.speed": val = cmi.student_preference.speed; break;
                        case "cmi.student_preference.text": val = cmi.student_preference.text; break;
                        case "adl.nav.request": adl.nav.request = value; val = true; break;
                        case "adl.nav.goto": adl.nav.goto = value; val = true; break;
                        case "cmi.assignments._submit": val = true; cmi.assignments_submit = value; break; //backward compat for SCO assignments
                        default: val = false; config.error = 401; break;
                    }
                }
                //config.pingAPI();
                // logCommit('SetValue',key,value);
                config.dirtyCommit = setTimeout(function () {
                    API_1484_11.Commit('');
                },60000);
                if (config.api_return_bool)
                    return val;
                else
                    return val.toString();
            } else {
                config.error = (config.init === true) ? 0 : 301;
                if (config.api_return_bool)
                    return false;
                else
                    return 'false';
            }
        },
        Terminate: function () {
            var msg = {

                status: 'Terminate',
                cmi: JSON.stringify(cmi)

            }
            
            if (typeof (jsBridge) !== 'undefined') {
                jsBridge.invokeAction(JSON.stringify(msg));
            } else {
                window.webkit.messageHandlers.invokeAction.postMessage(JSON.stringify(msg));
            }
            

            if (config.dirtyCommit !== null) {
                clearTimeout(config.dirtyCommit);
                config.dirtyCommit = null;
            }
            return "true";
        },
        Commit: function () {
            var msg = {

                status: 'Commit',
                cmi: JSON.stringify(cmi)

            }
            if (typeof (jsBridge) !== 'undefined') {
                jsBridge.invokeAction(JSON.stringify(msg));
            }else{
                window.webkit.messageHandlers.invokeAction.postMessage(JSON.stringify(msg));
            }
            return "true";
        },
        GetLastError: function () {
            return config.error;
        },
        GetErrorString: function (num) {
            var val = "";
            switch (num) {
                case 0: val = "No Error"; break;
                case 101: val = "General Exception"; break;
                case 201: val = "Invalid argument error"; break;
                case 202: val = "Element cannot have children"; break;
                case 203: val = "Element not an array. Cannot have count."; break;
                case 301: val = "Not initialized"; break;
                case 401: val = "Not implemented error"; break;
                case 402: val = "Invalid set value, element is a keyword"; break;
                case 403: val = "Element is read only"; break;
                case 404: val = "Element is write only"; break;
                case 405: val = "Incorrect Data Type"; break;
            }
            return val;

        },
        GetDiagnostic: function () {

        },
        getCourseInfo: function () {
            // this is a custom function for Thinkingcap LMS

            return cmi.courseinfo;
        }
    };

}
  





var GetCMIInteractions = function (key) {
    if (typeof (cmi.interactions) === 'undefined')
        cmi.interactions = [];
    if (key === "cmi.interactions._count") {
        return (typeof(cmi.interactions !== 'undefined')) ? cmi.interactions.length : 0;
    } else if (key === "cmi.interactions._children") {
        return '"id", "objectives", "time", "type", "correct_responses", "weighting", "learner_response", "result", "latency"';
    } else if (key.indexOf("objectives") > 0) {
        if (key.indexOf("count") > 0) {
            var x = key.split(".");
            var interaction = (function () { return eval("cmi.interactions[" + x[2] + "]"); })();
            if (typeof interaction === 'undefined') {
                return 401;
            } else {
                return interaction.objectives.length;
            }
        } else if (key.indexOf('id') > 0) {
            var x = key.split(".");
            var interaction = (function () { return eval("cmi.interactions[" + x[2] + "].objectives[" + x[4] + "].id"); })();
            return interaction;
        }
    } else if (key.indexOf('correct_responses') > 0) {
        if (key.indexOf('correct_responses._count') > 0) {
            var correct = 0;
            for (var xx = 0; xx < cmi.interactions.length; xx++) {
                if (cmi.interactions[xx].result === 'correct')
                    correct++;
            }
            return correct;
        }

    } else {
        var x = key.split(".");
        var property = x[3];
        var objective = (function () { return eval("cmi.interactions[" + x[2] + "]"); })();
        if (typeof objective === 'undefined') {
            return 201;
        } else {
            return objective[property];
        }
    }
};

var GetCMIObjectives = function (key) {
    //cmi.objectives._count = (typeof(cmi.objectives) !== 'undefined') ? cmi.objectives.length : 0;
    if (typeof (cmi.objectives) === 'undefined') {
        cmi.objectives = [];
    }
    if (key === "cmi.objectives._count") {
        return cmi.objectives.length;
    } else if (key === "cmi.objectives._children") {
        return "id,score,success_status,completion_status,description";
    } else {
        var x = key.split(".");
        var property = x[3];
        var p2 = (x[4]) ? x[4] : null;
        var isIndex = isNaN(x[2] * 1);
        if (isIndex === false) {
            var objective = (function () { return eval("cmi.objectives[" + x[2] + "]"); })();
        } else {
            var objective = $.grep(cmi.objectives, function (n, i) { return n.id === x[2] });
            objective = objective[0];
        }
        cmi.objectives._count = cmi.objectives.length;
        if (typeof objective === 'undefined') {
            return 201;
        } else {
            return (p2 !== null) ? objective[property][p2] : objective[property];
        }
    }
};


var GetCMIComments = function (key) {
    if (key === "cmi.comments_from_learner._count") {
        return cmi.comments_from_learner.comments.length;
    } else if (key === "cmi.comments_from_learner._children") {
        return "comment,location,timestamp";
    } else {
        var x = key.split(".");
        var property = x[3];
        var p2 = (x[4]) ? x[4] : null;
        var comment = (function () { return eval("cmi.comments_from_learner.comments[" + x[2] + "]"); })();
        if (typeof comment === 'undefined') {
            return 201;
        } else {
            return (p2 !== null) ? comment[property][p2] : comment[property];
        }
    }
};
var SetCMIInteractions = function (key, value) {
    cmi.interactions._count = cmi.interactions.length;
    if (key === "cmi.interactions._count") {
        return false;
    } else if (key.indexOf("objectives") > 0) {
        var x = key.split(".")[2];
        var objPos = key.split(".")[4];
        var objectives = eval("cmi.interactions[" + x + "].objectives");
        if (objPos > objectives.length) {
            config.error = 201;
            return false;
        } else {
            objectives.push({ 'id': value });
            return true;
        }
    } else if (key.indexOf('correct_responses') > 0 && key.indexOf('pattern') > 0) {
        var x = key.split(".")[2];
        var objPos = key.split(".")[4];
        var correct_responses = eval("cmi.interactions[" + x + "].correct_responses");
        if (objPos > correct_responses.length) {
            config.error = 201;
            return false;
        } else {
            correct_responses.push({ 'pattern': value });
            return true;
        }
    } else if (key === "cmi.interactions._children") {
        return false;
    } else {
        var x = key.split(".");
        var property = x[3];
        var objective = (function () { return eval("cmi.interactions[" + x[2] + "]"); })();
        if (typeof objective === 'undefined') {
            if (x[2] > cmi.interactions._count) {
                config.error = 201;
                return false;
            } else {
                switch (property) {
                    case 'id':
                        var objective = Interaction(value)
                        cmi.interactions.push(objective);
                        config.error = 0;
                        return true;
                    default:
                        var objective = Interaction('Interaction ' + guid());
                        objective[property] = value;
                        cmi.interactions.push(objective);
                        config.error = 0;
                        return true;
                }
            }
        } else {
            var returnval = true;
            switch (property) {
                case "id": objective[property] = value; config.error = 0; break;
                case "type": objective[property] = value; config.error = 0; break;
                case "time": objective[property] = value; config.error = 0; break;
                case "latency": objective[property] = value; config.error = 0; break;
                case "result": objective[property] = value; config.error = 0; break;
                case "learner_response": objective[property] = value; config.error = 0; break;
                case "weighting": objective[property] = value; config.error = 0; break;
                case "description": objective[property] = value; config.error = 0; break;
                case "timestamp": objective[property] = value; config.error = 0; break;
                default: returnval = false; config.error = 201; break;
            };
            cmi.interactions._count = cmi.interactions.length;
            return returnval;
        }
    }
};

SetInteractionType = function (interaction, value) {
    var returnvalue = true;
    switch (value) {
        case "true-false": interaction = value; returnvalue = true; break;
        case "choice": interaction = value; returnvalue = true; break;
        case "fill-in": interaction = value; returnvalue = true; break;
        case "matching": interaction = value; returnvalue = true; break;
        case "performance": interacton = value; returnvalue = true; break;
        case "sequencing": interaction = value; returnvalue = true; break;
        case "likert": interaction = value; returnvalue = true; break;
        case "numeric": interaction = value; returnvalue = true; break;
        default: returnvalue = false; break;
    }
    config.error = (returnvalue === true) ? 0 : 201;
    return returnvalue;

};

var SetCMIObjectives = function (key, value) {
    if (typeof (cmi.objectives) === 'undefined')
        cmi.objectives = [];
    cmi.objectives._count = cmi.objectives.length;
    if (key === "cmi.objectives._count") {
        return false;
    } else if (key === "cmi.objectives._children") {
        return false;
    } else {
        var x = key.split(".");
        var property = x[3];
        var p2 = x[4];
        var objective = eval("cmi.objectives[" + x[2] + "]");
        if (typeof objective === 'undefined') {
            if (x[2] > cmi.objectives.length) {
                config.error = 201;
                return false;
            } else {
                switch (property) {
                    case 'id':
                        var objective = Objective(value)
                        cmi.objectives.push(objective);
                        config.error = 0;
                        return true;
                    default:
                        var objective = Objective('Objective ' + guid())
                        cmi.objectives.push(objective);
                        config.error = 0;
                        return true;
                }
            }
        } else {
            var returnval = true;

            switch (property) {
                case "completion_status": objective[property] = value; config.error = 0; break;
                case "description": objective[property] = value; config.error = 0; break;
                case "id": objective[property] = value; config.error = 0; break;
                case "progress_measure": objective[property] = value; config.error = 0; break;
                case "success_status": objective[property] = value; config.error = 0; break;
                case "score": objective[property][p2] = value; config.error = 0; break;
                default: returnval = false; config.error = 201; break;
            };
            console.log(cmi.objectives);
            return returnval;
        }
    }
};

var SetCMIComments = function (key, value) {
    if (key === "cmi.comments_from_learner._count") {
        return false;
    } else if (key === "cmi.comments_from_learner._children") {
        return false;
    } else {
        var x = key.split(".");
        var property = x[3];
        var p2 = x[4];
        var comment = (function () { return cmi.comments_from_learner.comments[x[2]]; })();
        if (typeof comment === 'undefined') {
            if (x[2] > cmi.comments_from_learner._count) {
                config.error = 201;
                return false;
            } else {
                switch (property) {
                    case 'comment':
                        var comment = Comment(value);
                        cmi.comments_from_learner.comments.push(comment);
                        config.error = 0;
                        return true;
                    default: config.error = 201; return false;
                }
            }
        } else {
            var returnval = true;
            switch (property) {
                case "comment": comment[property] = value; config.error = 0; break;
                case "location": comment[property] = value; config.error = 0; break;
                case "timestamp": comment[property] = value; config.error = 0; break;
                default: returnval = false; config.error = 201; break;
            };

            return returnval;
        }
    }
};

var SetStatus = function (value) {
    var returnvalue = true;
    switch (value) {
        case "completed": cmi.completion_status = value; config.error = 0; break;
        case "incomplete": cmi.completion_status = value; config.error = 0; break;
        case "not attempted": cmi.completion_status = value; config.error = 0; break;
        case "unknown": cmi.completion_status = value; config.error = 0; break;
        default: return value = false; config.error = 405; break;

    }
    return returnvalue;
};

var CheckString = function (key, value) {
    if (typeof value === 'string') {
        switch (key) {
            case "cmi.location": cmi.location = value; break;
            default: break;
        }
        key = value;
        config.error = 0;
        return true;
    } else {
        config.error = 406;
        return false;
    }
};

var SetExit = function (value) {
    var returnvalue = (value === "time-out" || value === "suspend" || value === "logout" || value === "" || value == "normal") ? true : false;
    // var exit = API_1484_11.Commit('finish');
    if (returnvalue === true) { config.error = 0; cmi.exit = value; } else { config.error = 405; }
    return returnvalue;
};

var Interaction = function (id) {
    return {
        id: id,
        objectives: [],
        time: null,
        type: "",
        description: "",
        correct_responses: [],
        weighting: null,
        learner_response: "",
        result: "",
        latency: null,
        timestamp: null
    };

};
var Objective = function (id) {
    return {
        _children: "id,score,success_status,completion_status,description",
        id: id,
        score: {
            _children: "raw,min,max,scaled",
            raw: "",
            max: "",
            min: "",
            scaled: ""
        },
        description: "",
        completion_status: "unknown",
        success_status: "unknown",
        progress_measure: ""

    };
};

var Comment = function (c, t, l) {
    return {
        comment: c,
        timestamp: t,
        location: l
    };

};
// NOT supporting Objectives in 1.2 maybe later.
var objectives = function () {
    return {
        _children: "id,score,success_status,completion_status,description",
        id: id,
        score: {
            _children: "raw,min,max,scaled",
            raw: "",
            max: "",
            min: "",
            scaled: ""
        },
        description: "",
        completion_status: "unknown",
        success_status: "unknown"

    };
};

var guid = (function () {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return function () {
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            s4() + '-' + s4() + s4() + s4();
    };
})();

var API = API_1484_11;

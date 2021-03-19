﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using LeanCloud.Storage.Internal.Object;
using LeanCloud.Storage;

namespace LeanCloud.Engine {
    [ApiController]
    [Route("{1,1.1}")]
    [EnableCors(LCEngine.LCEngineCORS)]
    public class LCClassHookController : ControllerBase {
        private Dictionary<string, MethodInfo> ClassHooks => LCEngine.ClassHooks;

        [HttpPost("functions/{className}/{hookName}")]
        public async Task<object> Hook(string className, string hookName, JsonElement body) {
            try {
                LCLogger.Debug($"Hook: {className}#{hookName}");
                LCLogger.Debug(body.ToString());

                LCEngine.CheckHookKey(Request);

                string classHookName = GetClassHookName(className, hookName);
                if (ClassHooks.TryGetValue(classHookName, out MethodInfo mi)) {
                    Dictionary<string, object> dict = LCEngine.Decode(body);

                    LCObjectData objectData = LCObjectData.Decode(dict["object"] as Dictionary<string, object>);
                    objectData.ClassName = className;
                    LCObject obj = LCObject.Create(className);
                    obj.Merge(objectData);

                    // 避免死循环
                    if (hookName.StartsWith("before")) {
                        obj.DisableBeforeHook();
                    } else {
                        obj.DisableAfterHook();
                    }

                    LCUser user = null;
                    if (dict.TryGetValue("user", out object userObj) &&
                        userObj != null) {
                        user = new LCUser();
                        user.Merge(LCObjectData.Decode(userObj as Dictionary<string, object>));
                    }

                    LCClassHookRequest req = new LCClassHookRequest {
                        Object = obj,
                        CurrentUser = user
                    };

                    LCObject result = await LCEngine.Invoke(mi, req) as LCObject;
                    if (result != null) {
                        return LCCloud.Encode(result);
                    }
                }
                return default;
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        private static string GetClassHookName(string className, string hookName) {
            switch (hookName) {
                case "beforeSave":
                    return $"__before_save_for_{className}";
                case "afterSave":
                    return $"__after_save_for_{className}";
                case "beforeUpdate":
                    return $"__before_update_for_{className}";
                case "afterUpdate":
                    return $"__after_update_for_{className}";
                case "beforeDelete":
                    return $"__before_delete_for_{className}";
                case "afterDelete":
                    return $"__after_delete_for_{className}";
                default:
                    throw new Exception($"Error hook name: {hookName}");
            }
        }
    }
}

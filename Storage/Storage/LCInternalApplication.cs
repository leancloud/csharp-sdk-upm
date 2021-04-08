﻿using System;
using System.Collections.Generic;
using LeanCloud.Common;
using LeanCloud.Storage;
using LeanCloud.Storage.Internal.Http;
using LeanCloud.Storage.Internal.Storage;

namespace LeanCloud {
    /// <summary>
    /// LeanCloud Application
    /// </summary>
    public class LCInternalApplication {
        // SDK 版本号，用于 User-Agent 统计
        public const string SDKVersion = "0.7.3";

        // 接口版本号，用于接口版本管理
        internal const string APIVersion = "1.1";

        public static string AppId {
            get; private set;
        }

        public static string AppKey {
            get; private set;
        }

        public static string MasterKey {
            get; private set;
        }

        public static bool UseProduction {
            get; set;
        }

        public static LCAppRouter AppRouter {
            get; private set;
        }

        public static LCHttpClient HttpClient {
            get; private set;
        }

        public static bool UseMasterKey {
            get; set;
        }

        public static StorageController StorageController {
            get; set;
        }

        internal static Dictionary<string, string> AdditionalHeaders {
            get;
        } = new Dictionary<string, string>();

        public static void Initialize(string appId,
            string appKey,
            string server = null,
            string masterKey = null) {
            if (string.IsNullOrEmpty(appId)) {
                throw new ArgumentException(nameof(appId));
            }
            if (string.IsNullOrEmpty(appKey)) {
                throw new ArgumentException(nameof(appKey));
            }

            AppId = appId;
            AppKey = appKey;
            MasterKey = masterKey;

            // 注册 LeanCloud 内部子类化类型
            LCObject.RegisterSubclass(LCUser.CLASS_NAME, () => new LCUser());
            LCObject.RegisterSubclass(LCRole.CLASS_NAME, () => new LCRole());
            LCObject.RegisterSubclass(LCFile.CLASS_NAME, () => new LCFile());
            LCObject.RegisterSubclass(LCStatus.CLASS_NAME, () => new LCStatus());
            LCObject.RegisterSubclass(LCFriendshipRequest.CLASS_NAME, () => new LCFriendshipRequest());

            AppRouter = new LCAppRouter(appId, server);

            HttpClient = new LCHttpClient(appId, appKey, server, SDKVersion, APIVersion);
        }

        public static void AddHeader(string key, string value) {
            AdditionalHeaders.Add(key, value);
        }
    }
}

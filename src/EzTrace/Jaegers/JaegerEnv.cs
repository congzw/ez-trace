using System;

namespace EzTrace.Jaegers
{
    public class JaegerEnv
    {
        private JaegerEnv(){}

        // ReSharper disable once InconsistentNaming
        public string SPAN_STORAGE_TYPE { get; set; }
        // ReSharper disable once InconsistentNaming
        public string BADGER_EPHEMERAL { get; set; }
        // ReSharper disable once InconsistentNaming
        public string BADGER_DIRECTORY_VALUE { get; set; }
        // ReSharper disable once InconsistentNaming
        public string BADGER_DIRECTORY_KEY { get; set; }

        public void SetStorageEnv(EnvironmentVariableTarget target)
        {
            EnvVarHelper.SetEnvItem(this, _ => _.SPAN_STORAGE_TYPE, target);
        }

        public void ClearEnv(EnvironmentVariableTarget target)
        {
            EnvVarHelper.DeleteEnvItem(this, _ => _.SPAN_STORAGE_TYPE, target);
            EnvVarHelper.DeleteEnvItem(this, _ => _.BADGER_EPHEMERAL, target);
            EnvVarHelper.DeleteEnvItem(this, _ => _.BADGER_DIRECTORY_VALUE, target);
            EnvVarHelper.DeleteEnvItem(this, _ => _.BADGER_DIRECTORY_KEY, target);
        }

        public static JaegerEnv Instance = new JaegerEnv()
        {
            SPAN_STORAGE_TYPE = "memory"
            //SPAN_STORAGE_TYPE = "badger",
            //BADGER_EPHEMERAL = "false",
            //BADGER_DIRECTORY_VALUE = Path.Combine(storagePath, "data"),
            //BADGER_DIRECTORY_KEY = Path.Combine(storagePath, "key")
        };
    }
}
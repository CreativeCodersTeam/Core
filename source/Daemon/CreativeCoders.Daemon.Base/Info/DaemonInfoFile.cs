using CreativeCoders.Core.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace CreativeCoders.Daemon.Base.Info
{
    /// <summary>   A daemon information file. </summary>
    [PublicAPI]
    public class DaemonInfoFile
    {
        private readonly string _fileName;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the CreativeCoders.Daemon.Base.Info.DaemonInfoFile
        ///     class.
        /// </summary>
        ///
        /// <param name="fileName"> Filename of the daemon info file. </param>
        ///-------------------------------------------------------------------------------------------------
        public DaemonInfoFile(string fileName)
        {
            _fileName = fileName;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Loads the information from the file specified via <see cref="DaemonInfoFile(string)"/>.
        /// </summary>
        ///
        /// <returns>   The daemon info. </returns>
        ///-------------------------------------------------------------------------------------------------
        public DaemonInfo LoadInfo()
        {
            return JsonConvert.DeserializeObject<DaemonInfo>(FileSys.File.ReadAllText(_fileName),
                new JsonSerializerSettings {MissingMemberHandling = MissingMemberHandling.Ignore});
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves the daemon info to file specified via <see cref="DaemonInfoFile(string)"/>.
        /// </summary>
        ///
        /// <param name="daemonInfo">   Information describing the daemon. </param>
        ///-------------------------------------------------------------------------------------------------
        public void SaveInfo(DaemonInfo daemonInfo)
        {
            FileSys.File.WriteAllText(_fileName, JsonConvert.SerializeObject(daemonInfo, Formatting.Indented));
        }
    }
}
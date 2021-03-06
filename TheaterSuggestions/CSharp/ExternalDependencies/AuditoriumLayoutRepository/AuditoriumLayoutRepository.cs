﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ExternalDependencies.AuditoriumLayoutRepository
{
    public class AuditoriumLayoutRepository
    {
        private readonly Dictionary<string, AuditoriumDto> _repository = new Dictionary<string, AuditoriumDto>();

        public AuditoriumLayoutRepository()
        {
            var directoryName = $"{GetExecutingAssemblyDirectoryFullPath()}\\AuditoriumLayouts\\";

            foreach (var fileFullName in Directory.EnumerateFiles($"{directoryName}"))
                if (fileFullName.Contains("_theater.json"))
                {
                    var fileName = Path.GetFileName(fileFullName);

                    var eventId = Path.GetFileName(fileName.Split("-")[0]);
                    _repository[eventId] = JsonFile.ReadFromJsonFile<AuditoriumDto>(fileFullName);
                }
        }

        public AuditoriumDto GetAuditoriumLayoutFor(string showId)
        {
            if (_repository.ContainsKey(showId)) return _repository[showId];

            return new AuditoriumDto();
        }

        private static string GetExecutingAssemblyDirectoryFullPath()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetCallingAssembly().CodeBase);

            if (directoryName.StartsWith(@"file:\")) directoryName = directoryName.Substring(6);

            return directoryName;
        }
    }
}
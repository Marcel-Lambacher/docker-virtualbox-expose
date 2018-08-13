using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Docker.DotNet.Models;
using DockerVirtualBoxExpose.Common.Entities;

namespace DockerVirtualBoxExpose.DockerAgent.Docker
{
    public static class ExposedServiceLabelParser
    {
        public const string ExposedServiceLabel = "vm-expose";

        public static IEnumerable<ExposedService> GetExposedServicesFromContainer(ContainerListResponse container)
        {
            var exposedPorts = GetExposedServiceLabelValue(container);

            if (string.IsNullOrWhiteSpace(exposedPorts))
            {
                return GetAllExposedPorts(container);
            }

            return ParseExposedLabelValue(exposedPorts, container);
        }

        private static IEnumerable<ExposedService> ParseExposedLabelValue(string exposedPorts, ContainerListResponse container)
        {
            const char splitCharacter = ',';
            var stringPorts = RemoveAllWhitespaces(exposedPorts).Split(splitCharacter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var stringPort in stringPorts)
            {
                if (!int.TryParse(stringPort, out var port))
                {
                    continue;
                }

                yield return new ExposedService { ContainerId = container.ID, Port = port };
            }
        }

        private static string GetExposedServiceLabelValue(ContainerListResponse container)
        {
            var exposedLabel = container.Labels.FirstOrDefault(label => label.Key == ExposedServiceLabel);
            return exposedLabel.Value;
        }

        private static IEnumerable<ExposedService> GetAllExposedPorts(ContainerListResponse container)
        {
            return container.Ports
                .Where(containerPort => !string.IsNullOrWhiteSpace(containerPort.IP))
                .Select(containerPort => new ExposedService { ContainerId = container.ID, Port = containerPort.PublicPort });
        }

        private static string RemoveAllWhitespaces(string value)
        {
            return Regex.Replace(value, @"\s+", string.Empty);
        }
    }
}

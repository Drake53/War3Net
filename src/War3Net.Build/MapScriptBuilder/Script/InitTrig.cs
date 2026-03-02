using System;
using War3Net.Build.Script;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual bool ShouldRenderTrigger(Map map, TriggerDefinition triggerDefinition)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            return triggerDefinition.IsEnabled;
        }
    }
}
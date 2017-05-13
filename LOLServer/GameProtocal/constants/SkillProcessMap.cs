using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.constants
{
    public static class SkillProcessMap
    {
        private static Dictionary<int, ISkillProc> skillProcs = new Dictionary<int, ISkillProc>();

        static SkillProcessMap()
        {
            skillProcs.Add(-1, new AttackSkillProc());
        }

        public static bool hasSkillProc(int key)
        {
            if (skillProcs.ContainsKey(key))
                return true;

            return false;
        }

        public static ISkillProc GetSkillProc(int key)
        {
            return skillProcs[key];
        }
    }
}

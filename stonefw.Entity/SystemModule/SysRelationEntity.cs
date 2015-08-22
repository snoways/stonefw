using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;
using System.Collections.Generic;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_Relation")]
    public partial class SysRelationEntity : BaseEntity
    {
        [Field("ModuleId")]
        public string ModuleId { get; set; }
        [Field("FuncPointId")]
        public string FuncPointId { get; set; }
        [Field("Permissions")]
        public string Permissions { get; set; }

        public string ModuleName { get; set; }
        public string FuncPointName { get; set; }
        public string PermissionsName { get; set; }
        public List<string> PermissionList { get; set; }
        public List<string> PermissionListName { get; set; }
    }
}

using System;

namespace ViewModels
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.IsActive = true;

            this.IsDeleted = false;

            Id = Guid.NewGuid();
        }
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.BaseEntity),
        //    Name = Resources.Strings.BaseEntityKeys.Id)]
        public System.Guid Id { get; set; }

        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.BaseEntity),
        //    Name = Resources.Strings.BaseEntityKeys.InsertTime)]

        public System.DateTime InsertTime { get; set; }


        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.BaseEntity),
        //    Name = Resources.Strings.BaseEntityKeys.UpdateTime)]

        public System.DateTime? UpdateTime { get; set; }

        // [System.ComponentModel.DataAnnotations.Display
        //(ResourceType = typeof(Resources.BaseEntity),
        //Name = Resources.Strings.BaseEntityKeys.IsActive)]
        public bool IsActive { get; set; }

        //[System.ComponentModel.DataAnnotations.Display
        //	(ResourceType = typeof(Resources.BaseEntity),
        //	Name = Resources.Strings.BaseEntityKeys.IsDeleted)]
        public bool IsDeleted { get; set; }
    }
}

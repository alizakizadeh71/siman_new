using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class newsweb : BaseExtendedEntity
    {
        public newsweb()
        {
        }


        #region Title
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.news),
            Name = Resources.Model.Strings.newsKeys.Title)]
        #endregion
        public string Title { get; set; }

        #region newsText
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.news),
            Name = Resources.Model.Strings.newsKeys.newsText)]
        #endregion
        public string newsText { get; set; }

        #region StartDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.news),
            Name = Resources.Model.Strings.newsKeys.StartDate)]
        #endregion
        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        #region EndDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.news),
            Name = Resources.Model.Strings.newsKeys.EndDate)]
        #endregion
        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }
    }
}

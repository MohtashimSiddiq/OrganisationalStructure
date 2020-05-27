using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ruag.Client.Helpers.DataTemplateSelectors
{
    public class MultiSelectDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddButtonTemplate { get; set; }
        public DataTemplate ItemsTemplate { get; set; }

        public MultiSelectDataTemplateSelector()
        {
        }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }
            if (item is MultiSelectItem<OrgRoleDTO>)
            {
                MultiSelectItem<OrgRoleDTO> tempItem = item as MultiSelectItem<OrgRoleDTO>;
                if (tempItem.IsButton)
                {
                    return AddButtonTemplate;
                }
                else
                    return ItemsTemplate;
            }

            return null;

        }
    }
}

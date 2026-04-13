using System.Collections.Generic;
using System.Linq;
using Eughc.BaseUI;

namespace Eughc.Farm.UI {
    public class UIPlantStatList : UIBasePickerList {
        protected override List<object> GetDataForRefresh() =>
            (DataSource?.Invoke() ?? new()).Cast<object>().ToList();

        protected override UIBasePickerListElement CreatePickerElementUI(object data) {
            var el = Instantiate(uiPickerElementPrefab, container.transform) as UIPlantStatListElement;
            el.SetData(data, ((UIPlantStatItem)data).DisplayName);
            return el;
        }
    }
}
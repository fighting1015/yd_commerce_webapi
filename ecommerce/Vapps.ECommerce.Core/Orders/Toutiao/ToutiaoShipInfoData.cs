using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Vapps.ECommerce.Orders.Toutiao
{
    public static class ToutiaoShipInfoData
    {
        public static string ShipInfoDataString => "[{\"id\":7,\"name\":\"圆通快递\"},{\"id\":8,\"name\":\"申通快递\"},{\"id\":9,\"name\":\"韵达快递\"},{\"id\":10,\"name\":\"佳怡物流\"},{\"id\":11,\"name\":\"优速物流\"},{\"id\":12,\"name\":\"顺丰快递\"},{\"id\":13,\"name\":\"德邦快递\"},{\"id\":14,\"name\":\"天天快递\"},{\"id\":15,\"name\":\"中通速递\"},{\"id\":16,\"name\":\"全峰快递\"},{\"id\":17,\"name\":\"EMS\"},{\"id\":18,\"name\":\"微特派\"},{\"id\":19,\"name\":\"邮政国内小包\"},{\"id\":20,\"name\":\"百世汇通\"},{\"id\":21,\"name\":\"宅急送\"},{\"id\":22,\"name\":\"如风达\"},{\"id\":23,\"name\":\"增益速递\"},{\"id\":24,\"name\":\"电子券\"},{\"id\":25,\"name\":\"国通快递\"},{\"id\":26,\"name\":\"快捷快递\"},{\"id\":27,\"name\":\"E速宝\"},{\"id\":28,\"name\":\"云购商品\"},{\"id\":30,\"name\":\"京东快递\"},{\"id\":31,\"name\":\"万象物流\"},{\"id\":32,\"name\":\"安能物流\"},{\"id\":33,\"name\":\"佳运美物流\"},{\"id\":34,\"name\":\"联邦快递\"},{\"id\":35,\"name\":\"远成快递\"},{\"id\":36,\"name\":\"信丰物流\"},{\"id\":37,\"name\":\"黄马甲\"}]";

        public static ShipInfoData GetToutiaoShipInfoById(int id)
        {
            List<ShipInfoData> shipInfo = JsonConvert.DeserializeObject<List<ShipInfoData>>(ShipInfoDataString);

            return shipInfo.FirstOrDefault(t => t.Id == id);
        }

        public static ShipInfoData GetToutiaoShipInfoByName(string name)
        {
            List<ShipInfoData> shipInfo = JsonConvert.DeserializeObject<List<ShipInfoData>>(ShipInfoDataString);

            return shipInfo.FirstOrDefault(t => t.Name == name);
        }
    }

    public class ShipInfoData
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }
}

/*Список заказов – таблица со списком заказов. 
 * В ней должны быть данные о дате заказа, составе заказа (бренд, название, количество), общая сумма заказанных товаров.
Предусмотреть, что при изменении данных о товаре, данные в заказе меняться не должны.
*/
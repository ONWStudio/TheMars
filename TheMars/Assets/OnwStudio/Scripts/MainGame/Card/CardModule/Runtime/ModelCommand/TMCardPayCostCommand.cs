using System.Collections;
using System.Collections.Generic;
using Onw.ServiceLocator;
using UnityEngine;

namespace TM.Card.Runtime
{
    public class TMCardPayCostCommand : ITMCardCommand
    {
        public void Execute(TMCardModel order)
        {
            if (!ServiceLocator<PlayerManager>.TryGetService(out PlayerManager player)) return;
            
            foreach (TMCardCost cost in order.CardData.CardCosts)
            {
                switch (cost.ResourceKind)
                {
                    case TMResourceKind.MARS_LITHIUM:
                        player.MarsLithium -= cost.Cost;
                        break;
                    case TMResourceKind.CREDIT:
                        player.Credit -= cost.Cost;
                        break;
                    case TMResourceKind.STEEL:
                        player.Steel -= cost.Cost;
                        break;
                    case TMResourceKind.PLANTS:
                        player.Plants -= cost.Cost;
                        break;
                    case TMResourceKind.CLAY:
                        player.Clay -= cost.Cost;
                        break;
                    case TMResourceKind.ELECTRICITY:
                        player.Electricity -= cost.Cost;
                        break;
                    case TMResourceKind.POPULATION:
                        player.Population -= cost.Cost;
                        break;
                }
            }
        }
    }
}


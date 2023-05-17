using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.ResourceManager.Resources;

namespace Delta_Lake_Explorer.Models;
public class UserEnvironmentData
{
    public IEnumerable<SubscriptionResource>? subscriptions{get; set; }
}

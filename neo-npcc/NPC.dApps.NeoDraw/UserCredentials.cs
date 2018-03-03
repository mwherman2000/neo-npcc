using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApps.NeoDraw
{
    public interface NPCLevel0Basic { }
    public interface NPCLevel1Managed { }
    public interface NPCLevel2Persistable { }
    public interface NPCLevel3Deletable { }
    public interface NPCLevel4Collectible { }

    public class UserCredentials :   NPCLevel0Basic,
                                     NPCLevel1Managed,
                                     NPCLevel2Persistable,
                                     NPCLevel3Deletable,
                                     NPCLevel4Collectible
    {
        byte[] encodedUsername;
        byte[] encodedPassword;
    }
}

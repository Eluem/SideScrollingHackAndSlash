using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SideScrollingHackandSlash
{
    class actionTest001:FlowList
    {
        public actionTest001()
            : base()
        {
            positionFlowX.Enqueue(actionPosX.Left);
            positionFlowX.Enqueue(actionPosX.Mid);
            positionFlowX.Enqueue(actionPosX.Right);
            positionFlowX.Enqueue(actionPosX.Mid);
            positionFlowX.Enqueue(actionPosX.Left);
            positionFlowY.Enqueue(actionPosY.Mid);
            positionFlowY.Enqueue(actionPosY.Mid);
            positionFlowY.Enqueue(actionPosY.Mid);
            positionFlowY.Enqueue(actionPosY.Mid);
            positionFlowY.Enqueue(actionPosY.Mid);
        }
    }
}

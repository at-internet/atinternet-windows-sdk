using System.Collections.Generic;

namespace ATInternet
{
    #region Dispatcher
    internal class Dispatcher
    {
        #region Members

        private Tracker tracker { get; set; }

        #endregion

        #region Constructor

        internal Dispatcher(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        internal void Dispatch(params BusinessObject[] businessObjects)
        {
            List<BusinessObject> trackerObjs;
            foreach(BusinessObject businessObject in businessObjects)
            {
                businessObject.SetEvent();
                if (businessObject is Screen || businessObject is DynamicScreen)
                {   
                    trackerObjs = new List<BusinessObject>();
                    trackerObjs.AddRange(tracker.businessObjects.Values);

                    bool hasOrder = false;

                    foreach(BusinessObject obj in trackerObjs)
                    { 
                        if(((obj is OnAppAd && (obj as OnAppAd).Action == OnAppAdAction.View)
                            || obj is InternalSearch
                            || obj is Order
                            || obj is ScreenInfo)
                            && obj.timestamp <= businessObject.timestamp)
                        {
                            if(obj is Order)
                            {
                                hasOrder = true;
                            }

                            obj.SetEvent();
                            tracker.businessObjects.Remove(obj.id);
                        }
                    }

                    if(tracker.Cart.CartId != null)
                    {
                        if(businessObject is Screen && (businessObject as Screen).IsBasketScreen
                            || hasOrder)
                        {
                            tracker.Cart.SetEvent();
                        }
                    }
                }
                else if (businessObject is Gesture)
                {
                    trackerObjs = new List<BusinessObject>();
                    trackerObjs.AddRange(tracker.businessObjects.Values);

                    if ((businessObject as Gesture).Action == GestureAction.InternalSearch)
                    {
                        foreach(BusinessObject obj in trackerObjs)
                        {
                            if(obj is InternalSearch && obj.timestamp <= businessObject.timestamp)
                            {
                                obj.SetEvent();
                                tracker.businessObjects.Remove(obj.id);
                            }
                        }
                    }
                }

                tracker.businessObjects.Remove(businessObject.id);
                trackerObjs = new List<BusinessObject>();
                trackerObjs.AddRange(tracker.businessObjects.Values);

                foreach (BusinessObject obj in trackerObjs)
                {
                    if(obj is CustomObject && obj.timestamp <= businessObject.timestamp)
                    {
                        obj.SetEvent();
                        tracker.businessObjects.Remove(obj.id);
                    }
                }
            }

            if(Hit.GetHitType(tracker.buffer.volatileParameters, tracker.buffer.persistentParameters) == HitType.Screen)
            {
                TechnicalContext.ScreenName = Tool.AppendParameterValues("p", tracker.buffer.volatileParameters, tracker.buffer.persistentParameters);
                List<int[]> level2Positions = Tool.FindParameterPositions("s2", tracker.buffer.volatileParameters, tracker.buffer.persistentParameters);

                if(level2Positions.Count > 0)
                {
                    int[] indexes = level2Positions[0];
                    string result = indexes[0] == 0 ? tracker.buffer.volatileParameters[indexes[1]].value() : tracker.buffer.persistentParameters[indexes[1]].value();
                    TechnicalContext.Level2 = int.Parse(result);
                }
                else
                {
                    TechnicalContext.Level2 = 0;
                }
            }
            
            AddIdentifiedVisitorInfos();

            tracker.SetParam("stc", LifeCycle.GetMetrics(), Param.Type.JSON, new ParamOption() { Append = true, Encode = true });

            Builder builder = new Builder(tracker);
            tracker.buffer.volatileParameters.Clear();
            TrackerQueue.Instance.Add(builder);

            tracker.Context.Level2 = tracker.Context.Level2;
        }

        internal void AddIdentifiedVisitorInfos()
        {
            if(bool.Parse(tracker.configuration.parameters["persistIdentifiedVisitor"]))
            {
                object visitorIdNum = Tracker.LocalSettings.Values["ATVisitorNumeric"];
                object visitorIdString = Tracker.LocalSettings.Values["ATVisitorText"];
                object visitorCat = Tracker.LocalSettings.Values["ATVisitorCategory"];

                if(visitorIdNum != null)
                {
                    tracker.SetParam("an", visitorIdNum.ToString());
                }
                if (visitorIdString != null)
                {
                    tracker.SetParam("at", visitorIdString.ToString());
                }
                if (visitorCat != null)
                {
                    tracker.SetParam("ac", visitorCat.ToString());
                }
            }
        }
    }

    #endregion
}

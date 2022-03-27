using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMIUICXCore.Types
{
    public static class ICXMsgFactory
    {
        enum ParamSize { P4 = 4, P8 = 8 };

        private static readonly Dictionary<Tuple<string, string>, ParamSize> ParamsSize = new()
        {
            { new Tuple<string, string>("62", "00"), ParamSize.P4 },
            { new Tuple<string, string>("62", "01"), ParamSize.P4 },
            { new Tuple<string, string>("62", "02"), ParamSize.P4 },
            { new Tuple<string, string>("62", "1E"), ParamSize.P4 },
        };

        private class ICXMsgBase
        {
            public string SR { get; }
            public string TA { get; }
            public string SS { get; }
            public string TY { get; }

            public ICXMsgBase(string SR, string TA, string SS, string TY)
            {
                this.SR = SR;
                this.TA = TA;
                this.SS = SS;
                this.TY = TY;
            }
        }

        private class ICXMsg : ICXMsgBase, IICXMsg
        {
            public string Para1 { get; }
            public string Para2 { get; }

            public ICXMsg(string SR, string TA, string SS, string TY, string Para1, string Para2)
                : base(SR, TA, SS, TY)
            {
                this.Para1 = Para1;
                this.Para2 = Para2;
            }

            public CallEventSender GetCallEventSender() => default;

            public override string ToString()
            {
                string Ret = string.Empty;
                if (TA == "42" || TA == "62")
                {
                    switch (TY)
                    {
                        case "00":
                            Ret = $"Intercom Server online. Parameter: {Para1}, Software version: {Para2}";
                            break;
                        case "01":
                            Ret = $"Intercom Server offline. Parameter: {Para1}, Software version: {Para2}";
                            break;
                        case "02":
                            if (Para2 == "0000")
                                Ret = "All cards in normal operation";
                            else
                                Ret = $"Card failure. Parameter: {Para1}, Code: {Para2}";
                            break;
                        case "05":
                            Ret = $"Line fault, Subscriber: {Para1}, Code: {Para2}";
                            break;
                        case "10":
                            if (Para2.Length == 0)
                                Ret = $"End of conversation {Para1}";
                            else
                                Ret = $"End of conversation from {Para1} to {Para2}";
                            break;
                        case "11":
                            Ret = $"Conversation interrupted from {Para1} to {Para2}";
                            break;
                        case "12":
                            Ret = $"Loudspeaker conversation from {Para1} to {Para2}";
                            break;
                        case "13":
                            Ret = $"Conversation partner private from {Para1} to {Para2}";
                            break;
                        case "14":
                            Ret = $"Conversation partner busy from {Para1} to {Para2}";
                            break;
                        case "15":
                            Ret = $"Line fault from {Para1} ({Para2})";
                            break;
                        case "17":
                            Ret = Para2 switch
                            {
                                "0000" => $"End of privacy/follow me/secretary from {Para1}",
                                "0200" => $"Activation privacy by turning subscriber from {Para1}",
                                "0100" => $"Activation privacy with \"031\" from {Para1}",
                                _ => $"Activation follow me/secretary from {Para1} to {Para2}",
                            };
                            break;
                        case "19":
                            Ret = $"Transfer from {Para1} ({Para2})";
                            break;
                        case "1A":
                            Ret = $"Transfer (called subscriber \"button 9\") from {Para1} ({Para2})";
                            break;
                        case "18":
                            Ret = $"Transfer (calling subscriber \"button 9\") from {Para1} ({Para2})";
                            break;
                        case "1E":
                            Ret = $"Start-up message";
                            break;
                        case "23":
                            Ret = $"Subscriber not available from {Para1} to {Para2}";
                            break;
                    }
                }
                else if (TA == "5B" || TA == "7B")
                {
                    switch (TY)
                    {
                        case "21":
                            Ret = $"Call request \"call 1\" from {Para2} to control desk {Para1}";
                            break;
                        case "22":
                            Ret = $"Call request \"call 2\" from {Para2} to control desk {Para1}";
                            break;
                        case "23":
                            Ret = $"Call park function activated from {Para2} to control desk {Para1}";
                            break;
                        case "30":
                            Ret = $"Call park function deleted from {Para2} to control desk {Para1}";
                            break;
                        case "31":
                            Ret = $"Acknowledge, buzzer off from {Para2} to control desk {Para1}";
                            break;
                        case "35":
                            Ret = $"Call park function deleted by this control desk from {Para2} to control desk {Para1}";
                            break;
                        case "36":
                            Ret = $"Ackowledged by this control desk from {Para2} to control desk {Para1}";
                            break;
                        case "39":
                            Ret = $"Automatically conversation end from {Para2} to control desk {Para1}";
                            break;
                    }
                }

                string Params = string.Format("SR: {0} TA: {1} SS: {2} TY: {3} Para1: {4} Para2: {5}", SR, TA, SS, TY, Para1, Para2);

                if (string.IsNullOrEmpty(Ret))
                    return Params;

                return string.Format("{0} ({1})", Ret, Params);
            }
        }

        private class FirstCallICXMsg : ICXMsgBase, IICXMsg
        {
            public string CallNumber { get; }    // Call number control desk
            public string Subscriber { get; }    // Call number calling subscriber

            public FirstCallICXMsg(string SR, string TA, string SS, string TY, string CallNumber, string Subscriber)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Subscriber = Subscriber;
            }

            public CallEventSender GetCallEventSender() => new("call_event", CallTypeTag.FirstCall, Subscriber, CallNumber, DateTime.Now, 0);

            public override string ToString()
            {
                return string.Format("Call request \"call 1\" from {0} to control desk {1}", Subscriber, CallNumber);
            }
        }

        private class FirstCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            // Requirement: IAX card with firmware version min. 1.3 is required.
            public string CallNumber { get; }   // 8-digit call number of the subscriber (leading empty digits are filled with “F”)
            public string Interface { get; }    // 8-digit call number of the control desk which IAX interface is used (leading empty digits are filled with “F”)
            public string Subscriber { get; }   // 24-digit numeric call number of the SIP subscriber (leading empty digits are filled with “F”)
            public string CallerId { get; }     // Alphanumeric caller ID of the SIP subscriber (length variable)

            public FirstCallICXMsgEx(string SR, string TA, string SS, string TY, string CallNumber, string Interface, string Subscriber, string CallerId)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Interface = Interface;
                this.Subscriber = Subscriber;
                this.CallerId = CallerId;
            }

            public CallEventSender GetCallEventSender() => new CallEventSenderEx("call_event", CallTypeTag.FirstCall, Subscriber, Interface, CallNumber, CallerId, DateTime.Now, 0);

            public override string ToString()
            {
                if (CallerId.Length == 0)
                    return string.Format("Call request \"call 1\" from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
                return string.Format("Call request \"call 1\" from SIP subscriber ({1}){0} to control desk {2}, caller Id: '{3}'", Subscriber, Interface, CallNumber, CallerId);
            }
        }

        private class SecondCallICXMsg : ICXMsgBase, IICXMsg
        {
            public string CallNumber { get; }    // Call number control desk
            public string Subscriber { get; }    // Call number calling subscriber

            public SecondCallICXMsg(string SR, string TA, string SS, string TY, string CallNumber, string Subscriber)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Subscriber = Subscriber;
            }

            public CallEventSender GetCallEventSender() => new("call_event", CallTypeTag.SecondCall, Subscriber, CallNumber, DateTime.Now, 0);

            public override string ToString()
            {
                return string.Format("Call request \"call 2\" from {0} to control desk {1}", Subscriber, CallNumber);
            }
        }

        private class SecondCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            // Requirement: IAX card with firmware version min. 1.3 is required.
            public string CallNumber { get; }   // 8-digit call number of the subscriber (leading empty digits are filled with “F”)
            public string Interface { get; }    // 8-digit call number of the control desk which IAX interface is used (leading empty digits are filled with “F”)
            public string Subscriber { get; }   // 24-digit numeric call number of the SIP subscriber (leading empty digits are filled with “F”)
            public string CallerId { get; }     // Alphanumeric caller ID of the SIP subscriber (length variable)

            public SecondCallICXMsgEx(string SR, string TA, string SS, string TY, string CallNumber, string Interface, string Subscriber, string CallerId)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Interface = Interface;
                this.Subscriber = Subscriber;
                this.CallerId = CallerId;
            }

            public CallEventSender GetCallEventSender() => new CallEventSenderEx("call_event", CallTypeTag.SecondCall, Subscriber, Interface, CallNumber, CallerId, DateTime.Now, 0);

            public override string ToString()
            {
                if (CallerId.Length == 0)
                    return string.Format("Call request \"call 2\" from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
                return string.Format("Call request \"call 2\" from SIP subscriber ({1}){0} to control desk {2}, caller Id: '{3}'", Subscriber, Interface, CallNumber, CallerId);
            }
        }

        private class DeleteCallICXMsg : ICXMsgBase, IICXMsg
        {
            public string CallNumber { get; }    // Call number control desk
            public string Subscriber { get; }    // Call number calling subscriber

            public DeleteCallICXMsg(string SR, string TA, string SS, string TY, string CallNumber, string Subscriber)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Subscriber = Subscriber;
            }

            public CallEventSender GetCallEventSender() => new("call_event", CallTypeTag.DeleteCall, Subscriber, CallNumber, DateTime.Now, 0);

            public override string ToString()
            {
                if (CallNumber.Length == 0)
                    return string.Format("Call park function deleted from {0}", Subscriber);
                return string.Format("Call park function deleted from {0} to control desk {1}", Subscriber, CallNumber);
            }
        }

        private class DeleteCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            // Requirement: IAX card with firmware version min. 1.3 is required.
            public string CallNumber { get; }   // 8-digit call number of the subscriber (leading empty digits are filled with “F”)
            public string Interface { get; }    // 8-digit call number of the control desk which IAX interface is used (leading empty digits are filled with “F”)
            public string Subscriber { get; }   // 24-digit numeric call number of the SIP subscriber (leading empty digits are filled with “F”)
            public string CallerId { get; }     // Alphanumeric caller ID of the SIP subscriber (length variable)

            public DeleteCallICXMsgEx(string SR, string TA, string SS, string TY, string CallNumber, string Interface, string Subscriber, string CallerId)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Interface = Interface;
                this.Subscriber = Subscriber;
                this.CallerId = CallerId;
            }

            public CallEventSender GetCallEventSender() => new CallEventSenderEx("call_event", CallTypeTag.DeleteCall, Subscriber, Interface, CallNumber, CallerId, DateTime.Now, 0);

            public override string ToString()
            {
                if (CallNumber.Length == 0)
                    return string.Format("Call park function deleted from ({1}){0}", Subscriber, Interface);
                if (CallerId.Length == 0)
                    return string.Format("Call park function deleted from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
                return string.Format("Call park function deleted from SIP subscriber ({1}){0} to control desk {2}, caller Id: '{3}'", Subscriber, Interface, CallNumber, CallerId);
            }
        }

        private class AnswerCallICXMsg : ICXMsgBase, IICXMsg
        {
            public string Calling { get; }      // Call number calling subscriber
            public string Called { get; }       // Call number calledsubscriber

            public AnswerCallICXMsg(string SR, string TA, string SS, string TY, string Calling, string Called)
                : base(SR, TA, SS, TY)
            {
                this.Calling = Calling;
                this.Called = Called;
            }

            public CallEventSender GetCallEventSender() => new("call_event", CallTypeTag.AnswerCall, Called, Calling, DateTime.Now, 0);

            public override string ToString()
            {
                return string.Format("Loudspeaker conversation from {0} to control desk {1}", Calling, Called);
            }
        }

        private class AnswerCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            // Requirement: IAX card with firmware version min. 1.3 is required.
            public string CallNumber { get; }   // 8-digit call number of the subscriber (leading empty digits are filled with “F”)
            public string Interface { get; }    // 8-digit call number of the control desk which IAX interface is used (leading empty digits are filled with “F”)
            public string Subscriber { get; }   // 24-digit numeric call number of the SIP subscriber (leading empty digits are filled with “F”)
            public string CallerId { get; }     // Alphanumeric caller ID of the SIP subscriber (length variable)

            public AnswerCallICXMsgEx(string SR, string TA, string SS, string TY, string CallNumber, string Interface, string Subscriber, string CallerId)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Interface = Interface;
                this.Subscriber = Subscriber;
                this.CallerId = CallerId;
            }

            public CallEventSender GetCallEventSender() => new CallEventSenderEx("call_event", CallTypeTag.AnswerCall, Subscriber, Interface, CallNumber, CallerId, DateTime.Now, 0);

            public override string ToString()
            {
                if (CallerId.Length == 0)
                    return string.Format("Loudspeaker conversation from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
                return string.Format("Loudspeaker conversation from SIP subscriber ({1}){0} to control desk {2}, caller Id: '{3}'", Subscriber, Interface, CallNumber, CallerId);
            }
        }

        private class BusyCallICXMsg : ICXMsgBase, IICXMsg
        {
            public string Calling { get; }      // Call number calling subscriber
            public string Called { get; }       // Call number calledsubscriber

            public BusyCallICXMsg(string SR, string TA, string SS, string TY, string Calling, string Called)
                : base(SR, TA, SS, TY)
            {
                this.Calling = Calling;
                this.Called = Called;
            }

            public CallEventSender GetCallEventSender() => new("call_event", CallTypeTag.BusyCall, Called, Calling, DateTime.Now, 0);

            public override string ToString()
            {
                return string.Format("Conversation partner busy from {0} to control desk {1}", Calling, Called);
            }
        }

        private class BusyCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            // Requirement: IAX card with firmware version min. 1.3 is required.
            public string CallNumber { get; }   // 8-digit call number of the subscriber (leading empty digits are filled with “F”)
            public string Interface { get; }    // 8-digit call number of the control desk which IAX interface is used (leading empty digits are filled with “F”)
            public string Subscriber { get; }   // 24-digit numeric call number of the SIP subscriber (leading empty digits are filled with “F”)
            public string CallerId { get; }     // Alphanumeric caller ID of the SIP subscriber (length variable)

            public BusyCallICXMsgEx(string SR, string TA, string SS, string TY, string CallNumber, string Interface, string Subscriber, string CallerId)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Interface = Interface;
                this.Subscriber = Subscriber;
                this.CallerId = CallerId;
            }

            public CallEventSender GetCallEventSender() => new CallEventSenderEx("call_event", CallTypeTag.BusyCall, Subscriber, Interface, CallNumber, CallerId, DateTime.Now, 0);

            public override string ToString()
            {
                if (CallerId.Length == 0)
                    return string.Format("Conversation partner busy from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
                return string.Format("Conversation partner busy from SIP subscriber ({1}){0} to control desk {2}, caller Id: '{3}'", Subscriber, Interface, CallNumber, CallerId);
            }
        }

        private class EndCallICXMsg : ICXMsgBase, IICXMsg
        {
            public string Calling { get; }      // Call number calling subscriber
            public string Called { get; }       // Call number calledsubscriber

            public EndCallICXMsg(string SR, string TA, string SS, string TY, string Calling, string Called)
                : base(SR, TA, SS, TY)
            {
                this.Calling = Calling;
                this.Called = Called;
            }

            public CallEventSender GetCallEventSender() => new("call_event", CallTypeTag.EndCall, Called, Calling, DateTime.Now, 0);

            public override string ToString()
            {
                if (Called.Length == 0)
                    return string.Format("End of conversation from {0}", Calling);
                return string.Format("End of conversation from {0} to control desk {1}", Calling, Called);
            }
        }

        private class EndCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            // Requirement: IAX card with firmware version min. 1.3 is required.
            public string CallNumber { get; }   // 8-digit call number of the subscriber (leading empty digits are filled with “F”)
            public string Interface { get; }    // 8-digit call number of the control desk which IAX interface is used (leading empty digits are filled with “F”)
            public string Subscriber { get; }   // 24-digit numeric call number of the SIP subscriber (leading empty digits are filled with “F”)
            public string CallerId { get; }     // Alphanumeric caller ID of the SIP subscriber (length variable)

            public EndCallICXMsgEx(string SR, string TA, string SS, string TY, string CallNumber, string Interface, string Subscriber, string CallerId)
                : base(SR, TA, SS, TY)
            {
                this.CallNumber = CallNumber;
                this.Interface = Interface;
                this.Subscriber = Subscriber;
                this.CallerId = CallerId;
            }

            public CallEventSender GetCallEventSender() => new CallEventSenderEx("call_event", CallTypeTag.EndCall, Subscriber, Interface, CallNumber, CallerId, DateTime.Now, 0);

            public override string ToString()
            {
                if (CallNumber.Length == 0)
                    return string.Format("End of conversation ({1}){0}", Subscriber, Interface);
                if (CallerId.Length == 0)
                    return string.Format("End of conversation from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
                return string.Format("End of conversation from SIP subscriber ({1}){0} to control desk {2}, caller Id: '{3}'", Subscriber, Interface, CallNumber, CallerId);
            }
        }

        private class S0IncomingCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            public string Interface { get; }
            public string CallNumber { get; }
            public string Subscriber { get; }

            public S0IncomingCallICXMsgEx(string SR, string TA, string SS, string TY, string Interface, string CallNumber, string Subscriber)
                : base(SR, TA, SS, TY)
            {
                this.Interface = Interface;
                this.CallNumber = CallNumber;
                this.Subscriber = Subscriber;
            }

            public CallEventSender GetCallEventSender() => default;

            public override string ToString()
            {
                if (CallNumber.Length == 0)
                    return string.Format("S0 Incoming call from SIP subscriber ({1}){0}", Subscriber, Interface);
                return string.Format("S0 Incoming call from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
            }
        }

        private class S0OutgoingCallICXMsgEx : ICXMsgBase, IICXMsg
        {
            public string Interface { get; }
            public string CallNumber { get; }
            public string Subscriber { get; }

            public S0OutgoingCallICXMsgEx(string SR, string TA, string SS, string TY, string Interface, string CallNumber, string Subscriber)
                : base(SR, TA, SS, TY)
            {
                this.Interface = Interface;
                this.CallNumber = CallNumber;
                this.Subscriber = Subscriber;
            }

            public CallEventSender GetCallEventSender() => default;

            public override string ToString()
            {
                if (CallNumber.Length == 0)
                    return string.Format("S0 Outgoing call from SIP subscriber ({1}){0}", Subscriber, Interface);
                return string.Format("S0 Outgoing call from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
            }
        }

        private class S0OutgoingCallBusyICXMsgEx : ICXMsgBase, IICXMsg
        {
            public string Interface { get; }
            public string CallNumber { get; }
            public string Subscriber { get; }

            public S0OutgoingCallBusyICXMsgEx(string SR, string TA, string SS, string TY, string Interface, string CallNumber, string Subscriber)
                : base(SR, TA, SS, TY)
            {
                this.Interface = Interface;
                this.CallNumber = CallNumber;
                this.Subscriber = Subscriber;
            }

            public CallEventSender GetCallEventSender() => default;

            public override string ToString()
            {
                if (CallNumber.Length == 0)
                    return string.Format("S0 Outgoing call busy from SIP subscriber ({1}){0}", Subscriber, Interface);
                return string.Format("S0 Outgoing call busy from SIP subscriber ({1}){0} to control desk {2}", Subscriber, Interface, CallNumber);
            }
        }

        public static IICXMsg GetICXMessage(byte[] message)
        {
            string SR = $"{Convert.ToChar(message[0])}{Convert.ToChar(message[1])}";   // System number of receiver
            string TA = $"{Convert.ToChar(message[2])}{Convert.ToChar(message[3])}";   // Task
            string SS = $"{Convert.ToChar(message[4])}{Convert.ToChar(message[5])}";   // System number of sender

            if (TA.CompareTo("60") < 0)    // Short message
            {
                byte[] s = message.Skip(6).Take(8).ToArray();
                var Params = Params4X(s);
                var Para1 = Params.Item1;
                var Para2 = Params.Item2;
                string TY = $"{Convert.ToChar(message[14])}{Convert.ToChar(message[15])}";

                if (TA == "5B" && TY == "21")   // Call request “call 1”
                    return new FirstCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "5B" && TY == "22")   // Call request “call 2”
                    return new SecondCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "5B" && TY == "30")   // Call request/call park function deleted
                    return new DeleteCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "5B" && TY == "39")   // Automatically conversation end
                    return new DeleteCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "42" && TY == "10")   // End of conversation (subscriber free)
                    return new EndCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "42" && TY == "11")   // Conversation interrupted (transfer, conference)
                    return new EndCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "42" && TY == "12")   // Loudspeaking conversation
                    return new AnswerCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "42" && TY == "13")   // Conversation partner private
                    return new AnswerCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                if (TA == "42" && TY == "14")   // Conversation partner busy
                    return new BusyCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                return new ICXMsg(SR, TA, SS, TY, Para1, Para2);
            }
            else
            {
                string TY = $"{Convert.ToChar(message[6])}{Convert.ToChar(message[7])}";

                if (TA.CompareTo("80") < 0)    // Long message
                {
                    var psd = ParamsSize.FirstOrDefault(p => p.Key.Item1 == TA && p.Key.Item2 == TY);
                    ParamSize ps = ParamSize.P8;
                    if (!psd.Equals(default(KeyValuePair<Tuple<string, string>, ParamSize>)))
                        ps = psd.Value;

                    string Para1;
                    string Para2;
                    if (ps == ParamSize.P4)
                    {
                        byte[] s = message.Skip(8).ToArray();
                        var Params = Params4X(s);
                        Para1 = Params.Item1;
                        Para2 = Params.Item2;
                    }
                    else //if (ps == ParamSize.P8)
                    {
                        byte[] s = message.Skip(8).ToArray();
                        var Params = Params8X(s);
                        Para1 = Params.Item1;
                        Para2 = Params.Item2;
                    }

                    if (TA == "7B" && TY == "21")   // Call request “call 1”
                        return new FirstCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "7B" && TY == "22")   // Call request “call 2”
                        return new SecondCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "7B" && TY == "30")   // Call request/call park function deleted
                        return new DeleteCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "7B" && TY == "39")   // Automatically conversation end
                        return new DeleteCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "62" && TY == "10")   // End of conversation (subscriber free)
                        return new EndCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "62" && TY == "11")   // Conversation interrupted (transfer, conference)
                        return new EndCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "62" && TY == "12")   // Loudspeaking conversation
                        return new AnswerCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "62" && TY == "13")   // Conversation partner private
                        return new AnswerCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    if (TA == "62" && TY == "14")   // Conversation partner busy
                        return new BusyCallICXMsg(SR, TA, SS, TY, Para1, Para2);
                    return new ICXMsg(SR, TA, SS, TY, Para1, Para2);
                }
                else if (TA.CompareTo("90") < 0)    // Long message
                {
                    byte[] s = message.Skip(8).ToArray();
                    var Params = Params44X(s);
                    string Para1 = Params.Item1;
                    string Para2 = Params.Item2;
                    string Para3 = Params.Item3;

                    if (TA == "82" && TY == "61")   // Incoming call
                        return new S0IncomingCallICXMsgEx(SR, TA, SS, TY, Para2, Para1, Para3);
                    if (TA == "82" && TY == "62")   // Outgoing call
                        return new S0OutgoingCallICXMsgEx(SR, TA, SS, TY, Para2, Para1, Para3);
                    if (TA == "82" && TY == "64")   // Outgoing call busy
                        return new S0OutgoingCallBusyICXMsgEx(SR, TA, SS, TY, Para2, Para1, Para3);
                    return new ICXMsg(SR, TA, SS, TY, Para1 + Para2, Para3);
                }
                else if (TA.CompareTo("90") >= 0)    // SIP message
                {
                    string Subscriber = Para2Ext(new byte[] { message[8], message[9], message[10], message[11], message[12], message[13], message[14], message[15] });
                    string Interface = Para2Ext(new byte[] { message[16], message[17], message[18], message[19], message[20], message[21], message[22], message[23] });
                    string CallNumber = Para2Ext(new byte[] { message[24], message[25], message[26], message[27], message[28], message[29], message[30], message[31],
                                                     message[32], message[33], message[34], message[35], message[36], message[37], message[38], message[39],
                                                     message[40], message[41], message[42], message[43], message[44], message[45], message[46], message[47] }).TrimEnd('F');
                    var CallerIdB = new StringBuilder();
                    for (int i = 40; i < message.Length; i++)
                        CallerIdB.Append(Convert.ToChar(message[i]));
                    string CallerId = CallerIdB.ToString().TrimStart('F').TrimEnd('F');

                    if (TA == "9B" && TY == "21")   // Call request “call 1”
                        return new FirstCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "9B" && TY == "22")   // Call request “call 2”
                        return new SecondCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "9B" && TY == "30")   // Call request/call park function deleted
                        return new DeleteCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "9B" && TY == "39")   // Automatically conversation end
                        return new DeleteCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "92" && TY == "10")   // End of conversation (subscriber free)
                        return new EndCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "92" && TY == "11")   // Conversation interrupted (transfer, conference)
                        return new EndCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "92" && TY == "12")   // Loudspeaking conversation
                        return new AnswerCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "92" && TY == "13")   // Conversation partner private
                        return new AnswerCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    if (TA == "92" && TY == "14")   // Conversation partner busy
                        return new BusyCallICXMsgEx(SR, TA, SS, TY, Subscriber, Interface, CallNumber, CallerId);
                    return new ICXMsg(SR, TA, SS, TY, Subscriber, Interface);
                }
            }

            return default;
        }

        public static string Para2Ext(byte[] Para)
        {
            StringBuilder sb = new(Para.Length);

            foreach (byte b in Para)
                sb.Append(Convert.ToChar(b));

            string ret = sb.ToString();
            return ret.TrimStart('F');
        }

        public static Tuple<string, string> Params4X(byte[] param)
        {
            string Para1 = Para2Ext(new byte[] { param[0], param[1], param[2], param[3] });
            var Para2B = new StringBuilder();
            for (int i = 4; i < param.Length; i++)
                Para2B.Append(Convert.ToChar(param[i]));
            string Para2 = Para2B.ToString().TrimStart('F');

            return new Tuple<string, string>(Para1, Para2);
        }

        public static Tuple<string, string> Params8X(byte[] param)
        {
            string Para1 = Para2Ext(new byte[] { param[0], param[1], param[2], param[3], param[4], param[5], param[6], param[7] });
            var Para2B = new StringBuilder();
            for (int i = 8; i < param.Length; i++)
                Para2B.Append(Convert.ToChar(param[i]));
            string Para2 = Para2B.ToString().TrimStart('F');

            return new Tuple<string, string>(Para1, Para2);
        }

        public static Tuple<string, string, string> Params44X(byte[] param)
        {
            string Para1 = Para2Ext(new byte[] { param[0], param[1], param[2], param[3] });
            string Para2 = Para2Ext(new byte[] { param[4], param[5], param[6], param[7] });
            var Para3B = new StringBuilder();
            for (int i = 8; i < param.Length; i++)
                Para3B.Append(Convert.ToChar(param[i]));
            string Para3 = Para3B.ToString().TrimStart('F');

            return new Tuple<string, string, string>(Para1, Para2, Para3);
        }
    }
}

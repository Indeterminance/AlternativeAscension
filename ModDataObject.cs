// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(Moddata));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Moddata)serializer.Deserialize(reader);
// }
using NeedyEnums;

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using static NeedyMintsOverdose.MintyOverdosePatches;

namespace NeedyXML
{
    //HELP

    [XmlRoot(ElementName = "label")]
    public class Label
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;
    }

    [XmlRoot(ElementName = "description")]
    public class Description
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;
    }

    [XmlRoot(ElementName = "command")]
    public class Command
    {

        [XmlElement(ElementName = "label")]
        public Label Name;

        [XmlElement(ElementName = "description")]
        public Description Desc;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "commands")]
    public class Commands
    {

        [XmlElement(ElementName = "command")]
        public List<Command> Command;
    }

    [XmlRoot(ElementName = "ending")]
    public class Ending
    {

        [XmlElement(ElementName = "name")]
        public string Name;

        [XmlElement(ElementName = "osimai")]
        public string Osimai;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "endings")]
    public class Endings
    {

        [XmlElement(ElementName = "ending")]
        public List<Ending> Ending;
    }

    [XmlRoot(ElementName = "tweet")]
    public class Tweet
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;

        [XmlAttribute(AttributeName = "id")]
        public string Id = "None";

        [XmlAttribute(AttributeName = "user")]
        public string User;

        [XmlAttribute(AttributeName = "cmdid")]
        public string cmdId;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "kuso")]
    public class KusoRep
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;

        [XmlAttribute(AttributeName = "id")]
        public string Id = "None";

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "tweets")]
    public class Tweets
    {

        [XmlElement(ElementName = "tweet")]
        public List<Tweet> Tweet;
    }

    [XmlRoot(ElementName = "kusos")]
    public class KusoReps
    {

        [XmlElement(ElementName = "kuso")]
        public List<KusoRep> KusoRep;
    }

    [XmlRoot(ElementName = "jine")]
    public class Jine
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlAttribute(AttributeName = "speaker")]
        public string Speaker;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "jines")]
    public class Jines
    {

        [XmlElement(ElementName = "jine")]
        public List<Jine> Jine;
    }

    [XmlRoot(ElementName = "speak")]
    public class Speak
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "dialogue")]
    public class Dialogue
    {

        [XmlElement(ElementName = "speak")]
        public List<Speak> Speak;
    }

    [XmlRoot(ElementName = "msg")]
    public class Msg
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "chat")]
    public class Chat
    {

        [XmlElement(ElementName = "msg")]
        public List<Msg> Msg;
    }

    [XmlRoot(ElementName = "stream")]
    public class Stream
    {

        [XmlElement(ElementName = "dialogue")]
        public Dialogue Dialogue;

        [XmlElement(ElementName = "chat")]
        public Chat Chat;

        [XmlAttribute(AttributeName = "name")]
        public string Name;

        [XmlAttribute(AttributeName = "genre")]
        public string Genre;

        [XmlAttribute(AttributeName = "alphaType")]
        public string AlphaType;

        [XmlAttribute(AttributeName = "level")]
        public string AlphaLevel;

        [XmlAttribute(AttributeName = "actionType")]
        public string ActionType;

        [XmlAttribute(AttributeName = "cmdType")]
        public string CmdType;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "streams")]
    public class Streams
    {

        [XmlElement(ElementName = "stream")]
        public List<Stream> Stream;
    }

    [XmlRoot(ElementName = "title")]
    public class Title
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;
    }

    [XmlRoot(ElementName = "comment")]
    public class Comment
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "comments")]
    public class Comments
    {

        [XmlElement(ElementName = "comment")]
        public List<Comment> Comment;
    }

    [XmlRoot(ElementName = "thread")]
    public class Thread
    {

        [XmlElement(ElementName = "title")]
        public Title Title;

        [XmlElement(ElementName = "comments")]
        public Comments Comments;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "st")]
    public class ST
    {

        [XmlElement(ElementName = "thread")]
        public List<Thread> Threads;
    }

    [XmlRoot(ElementName = "string")]
    public class String
    {

        [XmlElement(ElementName = "bodyEN")]
        public string BodyEN;

        [XmlAttribute(AttributeName = "id")]
        public string Id;

        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "sysstrings")]
    public class Sysstrings
    {

        [XmlElement(ElementName = "string")]
        public List<String> Strings;
    }

    [XmlRoot(ElementName = "moddata")]
    public class ModData
    {
        [XmlElement(ElementName = "commands")]
        public Commands Commands;

        [XmlElement(ElementName = "endings")]
        public Endings Endings;

        [XmlElement(ElementName = "tweets")]
        public Tweets Tweets;

        [XmlElement(ElementName = "kusos")]
        public KusoReps KusoReps;

        [XmlElement(ElementName = "jines")]
        public Jines Jines;

        [XmlElement(ElementName = "streams")]
        public Streams Streams;

        [XmlElement(ElementName = "st")]
        public ST ST;

        [XmlElement(ElementName = "sysstrings")]
        public Sysstrings SysStrings;




        public Ending GetEndingByID(string id)
        {
            return Endings.Ending.Find(e => e.Id == id);
        }
        public Tweet GetTweetByID(string id)
        {
            return Tweets.Tweet.Find(t => t.Id == id);
        }
        public Jine GetJINEByID(string id)
        {
            return Jines.Jine.Find(j => j.Id == id);
        }
        public Msg GetMsgByAlpha(ModdedAlphaType alphaType, int level, string msgId)
        {
            Stream stream = Streams.Stream.Find(s => s.AlphaType == alphaType.ToString() && s.AlphaLevel == level.ToString());
            return stream.Chat.Msg.Find(m => m.Id == msgId);
        }
        public Speak GetSpeakByID(string streamId, int level, string id)
        {
            Stream stream = Streams.Stream.Find(s => s.AlphaType == streamId && s.AlphaLevel == level.ToString());
            return stream.Dialogue.Speak.Find(m => m.Id == id);
        }
    }


}

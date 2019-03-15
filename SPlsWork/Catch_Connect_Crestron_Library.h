namespace Catch_Connect_Crestron_Library;
        // class declarations
         class ChangeEventArgs;
         class DigitalEventHandler;
         class DigitalPulseEventHandler;
         class AnalogEventHandler;
         class SerialEventHandler;
         class CatchConnect;
     class ChangeEventArgs 
    {
        // class delegates

        // class events

        // class functions
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        SIMPLSHARPSTRING key[];
        INTEGER digitalValue;
        INTEGER analogValue;
        SIMPLSHARPSTRING stringValue[];
    };

     class DigitalEventHandler 
    {
        // class delegates
        delegate FUNCTION DigitalHandler ( SIMPLSHARPSTRING key , INTEGER value );

        // class events

        // class functions
        FUNCTION Initialize ( SIMPLSHARPSTRING key );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty DigitalHandler OnDigitalEvent;
    };

     class DigitalPulseEventHandler 
    {
        // class delegates
        delegate FUNCTION DigitalPulseHandler ( SIMPLSHARPSTRING key );

        // class events

        // class functions
        FUNCTION Initialize ( SIMPLSHARPSTRING key );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty DigitalPulseHandler OnDigitalPulseEvent;
    };

     class AnalogEventHandler 
    {
        // class delegates
        delegate FUNCTION AnalogHandler ( SIMPLSHARPSTRING key , INTEGER value );

        // class events

        // class functions
        FUNCTION Initialize ( SIMPLSHARPSTRING key );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty AnalogHandler OnAnalogEvent;
    };

     class SerialEventHandler 
    {
        // class delegates
        delegate FUNCTION SerialHandler ( SIMPLSHARPSTRING key , SIMPLSHARPSTRING value );

        // class events

        // class functions
        FUNCTION Initialize ( SIMPLSHARPSTRING key );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty SerialHandler OnSerialEvent;
    };

    static class CatchConnect 
    {
        // class delegates
        delegate FUNCTION ServiceHandler ( INTEGER online , INTEGER statusValue , SIMPLSHARPSTRING statusString );

        // class events

        // class functions
        static FUNCTION InitializeDigital ( SIMPLSHARPSTRING key );
        static FUNCTION SetDigitalValue ( SIMPLSHARPSTRING key , INTEGER value );
        static FUNCTION InitializeAnalog ( SIMPLSHARPSTRING key );
        static FUNCTION SetAnalogValue ( SIMPLSHARPSTRING key , INTEGER value );
        static FUNCTION InitializeSerial ( SIMPLSHARPSTRING key );
        static FUNCTION SetSerialValue ( SIMPLSHARPSTRING key , SIMPLSHARPSTRING value );
        static FUNCTION InitializeService ( INTEGER port );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        static ChangeEventArgs changeEventArgs;
        static INTEGER serviceStatusValue;
        static LONG_INTEGER debug;

        // class properties
        DelegateProperty ServiceHandler OnServiceEvent;
    };


﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OPS.GetPostinfoServices {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="IranStandardAddress", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class IranStandardAddress : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long TraceIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProvinceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TownShipField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ZoneField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string VillageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LocalityTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LocalityNameField;
        
        private int LocalityCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubLocalityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StreetField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Street2Field;
        
        private double HouseNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FloorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SideFloorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BuildingNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostCodeField;
        
        private int ErrorCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorMessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public long TraceID {
            get {
                return this.TraceIDField;
            }
            set {
                if ((this.TraceIDField.Equals(value) != true)) {
                    this.TraceIDField = value;
                    this.RaisePropertyChanged("TraceID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Province {
            get {
                return this.ProvinceField;
            }
            set {
                if ((object.ReferenceEquals(this.ProvinceField, value) != true)) {
                    this.ProvinceField = value;
                    this.RaisePropertyChanged("Province");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string TownShip {
            get {
                return this.TownShipField;
            }
            set {
                if ((object.ReferenceEquals(this.TownShipField, value) != true)) {
                    this.TownShipField = value;
                    this.RaisePropertyChanged("TownShip");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Zone {
            get {
                return this.ZoneField;
            }
            set {
                if ((object.ReferenceEquals(this.ZoneField, value) != true)) {
                    this.ZoneField = value;
                    this.RaisePropertyChanged("Zone");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string Village {
            get {
                return this.VillageField;
            }
            set {
                if ((object.ReferenceEquals(this.VillageField, value) != true)) {
                    this.VillageField = value;
                    this.RaisePropertyChanged("Village");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string LocalityType {
            get {
                return this.LocalityTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.LocalityTypeField, value) != true)) {
                    this.LocalityTypeField = value;
                    this.RaisePropertyChanged("LocalityType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string LocalityName {
            get {
                return this.LocalityNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LocalityNameField, value) != true)) {
                    this.LocalityNameField = value;
                    this.RaisePropertyChanged("LocalityName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=7)]
        public int LocalityCode {
            get {
                return this.LocalityCodeField;
            }
            set {
                if ((this.LocalityCodeField.Equals(value) != true)) {
                    this.LocalityCodeField = value;
                    this.RaisePropertyChanged("LocalityCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string SubLocality {
            get {
                return this.SubLocalityField;
            }
            set {
                if ((object.ReferenceEquals(this.SubLocalityField, value) != true)) {
                    this.SubLocalityField = value;
                    this.RaisePropertyChanged("SubLocality");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string Street {
            get {
                return this.StreetField;
            }
            set {
                if ((object.ReferenceEquals(this.StreetField, value) != true)) {
                    this.StreetField = value;
                    this.RaisePropertyChanged("Street");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string Street2 {
            get {
                return this.Street2Field;
            }
            set {
                if ((object.ReferenceEquals(this.Street2Field, value) != true)) {
                    this.Street2Field = value;
                    this.RaisePropertyChanged("Street2");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=11)]
        public double HouseNumber {
            get {
                return this.HouseNumberField;
            }
            set {
                if ((this.HouseNumberField.Equals(value) != true)) {
                    this.HouseNumberField = value;
                    this.RaisePropertyChanged("HouseNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=12)]
        public string Floor {
            get {
                return this.FloorField;
            }
            set {
                if ((object.ReferenceEquals(this.FloorField, value) != true)) {
                    this.FloorField = value;
                    this.RaisePropertyChanged("Floor");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=13)]
        public string SideFloor {
            get {
                return this.SideFloorField;
            }
            set {
                if ((object.ReferenceEquals(this.SideFloorField, value) != true)) {
                    this.SideFloorField = value;
                    this.RaisePropertyChanged("SideFloor");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=14)]
        public string BuildingName {
            get {
                return this.BuildingNameField;
            }
            set {
                if ((object.ReferenceEquals(this.BuildingNameField, value) != true)) {
                    this.BuildingNameField = value;
                    this.RaisePropertyChanged("BuildingName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=15)]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=16)]
        public string PostCode {
            get {
                return this.PostCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.PostCodeField, value) != true)) {
                    this.PostCodeField = value;
                    this.RaisePropertyChanged("PostCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=17)]
        public int ErrorCode {
            get {
                return this.ErrorCodeField;
            }
            set {
                if ((this.ErrorCodeField.Equals(value) != true)) {
                    this.ErrorCodeField = value;
                    this.RaisePropertyChanged("ErrorCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=18)]
        public string ErrorMessage {
            get {
                return this.ErrorMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorMessageField, value) != true)) {
                    this.ErrorMessageField = value;
                    this.RaisePropertyChanged("ErrorMessage");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GetPostinfoServices.postServicesSoap")]
    public interface postServicesSoap {
        
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetAddressByPostalCode", ReplyAction="*")]
        OPS.GetPostinfoServices.GetAddressByPostalCodeResponse GetAddressByPostalCode(OPS.GetPostinfoServices.GetAddressByPostalCodeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetAddressByPostalCode", ReplyAction="*")]
        System.Threading.Tasks.Task<OPS.GetPostinfoServices.GetAddressByPostalCodeResponse> GetAddressByPostalCodeAsync(OPS.GetPostinfoServices.GetAddressByPostalCodeRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetAddressByPostalCodeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetAddressByPostalCode", Namespace="http://tempuri.org/", Order=0)]
        public OPS.GetPostinfoServices.GetAddressByPostalCodeRequestBody Body;
        
        public GetAddressByPostalCodeRequest() {
        }
        
        public GetAddressByPostalCodeRequest(OPS.GetPostinfoServices.GetAddressByPostalCodeRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetAddressByPostalCodeRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string password;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string postalcode;
        
        public GetAddressByPostalCodeRequestBody() {
        }
        
        public GetAddressByPostalCodeRequestBody(string username, string password, string postalcode) {
            this.username = username;
            this.password = password;
            this.postalcode = postalcode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetAddressByPostalCodeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetAddressByPostalCodeResponse", Namespace="http://tempuri.org/", Order=0)]
        public OPS.GetPostinfoServices.GetAddressByPostalCodeResponseBody Body;
        
        public GetAddressByPostalCodeResponse() {
        }
        
        public GetAddressByPostalCodeResponse(OPS.GetPostinfoServices.GetAddressByPostalCodeResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetAddressByPostalCodeResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public OPS.GetPostinfoServices.IranStandardAddress GetAddressByPostalCodeResult;
        
        public GetAddressByPostalCodeResponseBody() {
        }
        
        public GetAddressByPostalCodeResponseBody(OPS.GetPostinfoServices.IranStandardAddress GetAddressByPostalCodeResult) {
            this.GetAddressByPostalCodeResult = GetAddressByPostalCodeResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface postServicesSoapChannel : OPS.GetPostinfoServices.postServicesSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class postServicesSoapClient : System.ServiceModel.ClientBase<OPS.GetPostinfoServices.postServicesSoap>, OPS.GetPostinfoServices.postServicesSoap {
        
        public postServicesSoapClient() {
        }
        
        public postServicesSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public postServicesSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public postServicesSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public postServicesSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        OPS.GetPostinfoServices.GetAddressByPostalCodeResponse OPS.GetPostinfoServices.postServicesSoap.GetAddressByPostalCode(OPS.GetPostinfoServices.GetAddressByPostalCodeRequest request) {
            return base.Channel.GetAddressByPostalCode(request);
        }
        
        public OPS.GetPostinfoServices.IranStandardAddress GetAddressByPostalCode(string username, string password, string postalcode) {
            OPS.GetPostinfoServices.GetAddressByPostalCodeRequest inValue = new OPS.GetPostinfoServices.GetAddressByPostalCodeRequest();
            inValue.Body = new OPS.GetPostinfoServices.GetAddressByPostalCodeRequestBody();
            inValue.Body.username = username;
            inValue.Body.password = password;
            inValue.Body.postalcode = postalcode;
            OPS.GetPostinfoServices.GetAddressByPostalCodeResponse retVal = ((OPS.GetPostinfoServices.postServicesSoap)(this)).GetAddressByPostalCode(inValue);
            return retVal.Body.GetAddressByPostalCodeResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<OPS.GetPostinfoServices.GetAddressByPostalCodeResponse> OPS.GetPostinfoServices.postServicesSoap.GetAddressByPostalCodeAsync(OPS.GetPostinfoServices.GetAddressByPostalCodeRequest request) {
            return base.Channel.GetAddressByPostalCodeAsync(request);
        }
        
        public System.Threading.Tasks.Task<OPS.GetPostinfoServices.GetAddressByPostalCodeResponse> GetAddressByPostalCodeAsync(string username, string password, string postalcode) {
            OPS.GetPostinfoServices.GetAddressByPostalCodeRequest inValue = new OPS.GetPostinfoServices.GetAddressByPostalCodeRequest();
            inValue.Body = new OPS.GetPostinfoServices.GetAddressByPostalCodeRequestBody();
            inValue.Body.username = username;
            inValue.Body.password = password;
            inValue.Body.postalcode = postalcode;
            return ((OPS.GetPostinfoServices.postServicesSoap)(this)).GetAddressByPostalCodeAsync(inValue);
        }
    }
}
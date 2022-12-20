// Code generated by protoc-gen-go. DO NOT EDIT.
// versions:
// 	protoc-gen-go v1.28.1
// 	protoc        v3.12.4
// source: packets.proto

package grpc

import (
	empty "github.com/golang/protobuf/ptypes/empty"
	protoreflect "google.golang.org/protobuf/reflect/protoreflect"
	protoimpl "google.golang.org/protobuf/runtime/protoimpl"
	reflect "reflect"
	sync "sync"
)

const (
	// Verify that this generated code is sufficiently up-to-date.
	_ = protoimpl.EnforceVersion(20 - protoimpl.MinVersion)
	// Verify that runtime/protoimpl is sufficiently up-to-date.
	_ = protoimpl.EnforceVersion(protoimpl.MaxVersion - 20)
)

type PacketModel struct {
	state         protoimpl.MessageState
	sizeCache     protoimpl.SizeCache
	unknownFields protoimpl.UnknownFields

	Ip       uint32 `protobuf:"varint,1,opt,name=ip,proto3" json:"ip,omitempty"`
	Size     uint32 `protobuf:"varint,2,opt,name=size,proto3" json:"size,omitempty"`
	Protocol uint32 `protobuf:"varint,3,opt,name=protocol,proto3" json:"protocol,omitempty"`
	Status   uint32 `protobuf:"varint,4,opt,name=status,proto3" json:"status,omitempty"`
}

func (x *PacketModel) Reset() {
	*x = PacketModel{}
	if protoimpl.UnsafeEnabled {
		mi := &file_packets_proto_msgTypes[0]
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		ms.StoreMessageInfo(mi)
	}
}

func (x *PacketModel) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*PacketModel) ProtoMessage() {}

func (x *PacketModel) ProtoReflect() protoreflect.Message {
	mi := &file_packets_proto_msgTypes[0]
	if protoimpl.UnsafeEnabled && x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use PacketModel.ProtoReflect.Descriptor instead.
func (*PacketModel) Descriptor() ([]byte, []int) {
	return file_packets_proto_rawDescGZIP(), []int{0}
}

func (x *PacketModel) GetIp() uint32 {
	if x != nil {
		return x.Ip
	}
	return 0
}

func (x *PacketModel) GetSize() uint32 {
	if x != nil {
		return x.Size
	}
	return 0
}

func (x *PacketModel) GetProtocol() uint32 {
	if x != nil {
		return x.Protocol
	}
	return 0
}

func (x *PacketModel) GetStatus() uint32 {
	if x != nil {
		return x.Status
	}
	return 0
}

type AddPacketsRequest struct {
	state         protoimpl.MessageState
	sizeCache     protoimpl.SizeCache
	unknownFields protoimpl.UnknownFields

	Packets []*PacketModel `protobuf:"bytes,1,rep,name=packets,proto3" json:"packets,omitempty"`
}

func (x *AddPacketsRequest) Reset() {
	*x = AddPacketsRequest{}
	if protoimpl.UnsafeEnabled {
		mi := &file_packets_proto_msgTypes[1]
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		ms.StoreMessageInfo(mi)
	}
}

func (x *AddPacketsRequest) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*AddPacketsRequest) ProtoMessage() {}

func (x *AddPacketsRequest) ProtoReflect() protoreflect.Message {
	mi := &file_packets_proto_msgTypes[1]
	if protoimpl.UnsafeEnabled && x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use AddPacketsRequest.ProtoReflect.Descriptor instead.
func (*AddPacketsRequest) Descriptor() ([]byte, []int) {
	return file_packets_proto_rawDescGZIP(), []int{1}
}

func (x *AddPacketsRequest) GetPackets() []*PacketModel {
	if x != nil {
		return x.Packets
	}
	return nil
}

var File_packets_proto protoreflect.FileDescriptor

var file_packets_proto_rawDesc = []byte{
	0x0a, 0x0d, 0x70, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x2e, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x12,
	0x07, 0x70, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x1a, 0x1b, 0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65,
	0x2f, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62, 0x75, 0x66, 0x2f, 0x65, 0x6d, 0x70, 0x74, 0x79, 0x2e,
	0x70, 0x72, 0x6f, 0x74, 0x6f, 0x22, 0x65, 0x0a, 0x0b, 0x50, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x4d,
	0x6f, 0x64, 0x65, 0x6c, 0x12, 0x0e, 0x0a, 0x02, 0x69, 0x70, 0x18, 0x01, 0x20, 0x01, 0x28, 0x0d,
	0x52, 0x02, 0x69, 0x70, 0x12, 0x12, 0x0a, 0x04, 0x73, 0x69, 0x7a, 0x65, 0x18, 0x02, 0x20, 0x01,
	0x28, 0x0d, 0x52, 0x04, 0x73, 0x69, 0x7a, 0x65, 0x12, 0x1a, 0x0a, 0x08, 0x70, 0x72, 0x6f, 0x74,
	0x6f, 0x63, 0x6f, 0x6c, 0x18, 0x03, 0x20, 0x01, 0x28, 0x0d, 0x52, 0x08, 0x70, 0x72, 0x6f, 0x74,
	0x6f, 0x63, 0x6f, 0x6c, 0x12, 0x16, 0x0a, 0x06, 0x73, 0x74, 0x61, 0x74, 0x75, 0x73, 0x18, 0x04,
	0x20, 0x01, 0x28, 0x0d, 0x52, 0x06, 0x73, 0x74, 0x61, 0x74, 0x75, 0x73, 0x22, 0x43, 0x0a, 0x11,
	0x41, 0x64, 0x64, 0x50, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x52, 0x65, 0x71, 0x75, 0x65, 0x73,
	0x74, 0x12, 0x2e, 0x0a, 0x07, 0x70, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x18, 0x01, 0x20, 0x03,
	0x28, 0x0b, 0x32, 0x14, 0x2e, 0x70, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x2e, 0x50, 0x61, 0x63,
	0x6b, 0x65, 0x74, 0x4d, 0x6f, 0x64, 0x65, 0x6c, 0x52, 0x07, 0x70, 0x61, 0x63, 0x6b, 0x65, 0x74,
	0x73, 0x32, 0x4b, 0x0a, 0x07, 0x50, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x12, 0x40, 0x0a, 0x0a,
	0x41, 0x64, 0x64, 0x50, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x12, 0x1a, 0x2e, 0x70, 0x61, 0x63,
	0x6b, 0x65, 0x74, 0x73, 0x2e, 0x41, 0x64, 0x64, 0x50, 0x61, 0x63, 0x6b, 0x65, 0x74, 0x73, 0x52,
	0x65, 0x71, 0x75, 0x65, 0x73, 0x74, 0x1a, 0x16, 0x2e, 0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x2e,
	0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62, 0x75, 0x66, 0x2e, 0x45, 0x6d, 0x70, 0x74, 0x79, 0x42, 0x1c,
	0x5a, 0x08, 0x70, 0x6b, 0x67, 0x2f, 0x67, 0x72, 0x70, 0x63, 0xaa, 0x02, 0x0f, 0x4e, 0x6d, 0x74,
	0x2e, 0x47, 0x72, 0x70, 0x63, 0x2e, 0x50, 0x72, 0x6f, 0x74, 0x6f, 0x73, 0x62, 0x06, 0x70, 0x72,
	0x6f, 0x74, 0x6f, 0x33,
}

var (
	file_packets_proto_rawDescOnce sync.Once
	file_packets_proto_rawDescData = file_packets_proto_rawDesc
)

func file_packets_proto_rawDescGZIP() []byte {
	file_packets_proto_rawDescOnce.Do(func() {
		file_packets_proto_rawDescData = protoimpl.X.CompressGZIP(file_packets_proto_rawDescData)
	})
	return file_packets_proto_rawDescData
}

var file_packets_proto_msgTypes = make([]protoimpl.MessageInfo, 2)
var file_packets_proto_goTypes = []interface{}{
	(*PacketModel)(nil),       // 0: packets.PacketModel
	(*AddPacketsRequest)(nil), // 1: packets.AddPacketsRequest
	(*empty.Empty)(nil),       // 2: google.protobuf.Empty
}
var file_packets_proto_depIdxs = []int32{
	0, // 0: packets.AddPacketsRequest.packets:type_name -> packets.PacketModel
	1, // 1: packets.Packets.AddPackets:input_type -> packets.AddPacketsRequest
	2, // 2: packets.Packets.AddPackets:output_type -> google.protobuf.Empty
	2, // [2:3] is the sub-list for method output_type
	1, // [1:2] is the sub-list for method input_type
	1, // [1:1] is the sub-list for extension type_name
	1, // [1:1] is the sub-list for extension extendee
	0, // [0:1] is the sub-list for field type_name
}

func init() { file_packets_proto_init() }
func file_packets_proto_init() {
	if File_packets_proto != nil {
		return
	}
	if !protoimpl.UnsafeEnabled {
		file_packets_proto_msgTypes[0].Exporter = func(v interface{}, i int) interface{} {
			switch v := v.(*PacketModel); i {
			case 0:
				return &v.state
			case 1:
				return &v.sizeCache
			case 2:
				return &v.unknownFields
			default:
				return nil
			}
		}
		file_packets_proto_msgTypes[1].Exporter = func(v interface{}, i int) interface{} {
			switch v := v.(*AddPacketsRequest); i {
			case 0:
				return &v.state
			case 1:
				return &v.sizeCache
			case 2:
				return &v.unknownFields
			default:
				return nil
			}
		}
	}
	type x struct{}
	out := protoimpl.TypeBuilder{
		File: protoimpl.DescBuilder{
			GoPackagePath: reflect.TypeOf(x{}).PkgPath(),
			RawDescriptor: file_packets_proto_rawDesc,
			NumEnums:      0,
			NumMessages:   2,
			NumExtensions: 0,
			NumServices:   1,
		},
		GoTypes:           file_packets_proto_goTypes,
		DependencyIndexes: file_packets_proto_depIdxs,
		MessageInfos:      file_packets_proto_msgTypes,
	}.Build()
	File_packets_proto = out.File
	file_packets_proto_rawDesc = nil
	file_packets_proto_goTypes = nil
	file_packets_proto_depIdxs = nil
}

import { gql } from 'apollo-angular';
import { Injectable } from '@angular/core';
import * as Apollo from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: string;
  String: string;
  Boolean: boolean;
  Int: number;
  Float: number;
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: any;
  /** The `Long` scalar type represents non-fractional signed whole 64-bit numeric values. Long can represent values between -(2^63) and 2^63 - 1. */
  Long: any;
  UUID: any;
};

export enum ApplyPolicy {
  AfterResolver = 'AFTER_RESOLVER',
  BeforeResolver = 'BEFORE_RESOLVER'
}

export type CreateIpFilterCommandInput = {
  comment?: InputMaybe<Scalars['String']>;
  filterAction: IpFilterAction;
  ip: Scalars['Long'];
  userId: Scalars['UUID'];
};

export type DeleteIpFilterCommandInput = {
  ipFilterId: Scalars['UUID'];
};

export type DeviceDto = {
  __typename?: 'DeviceDto';
  createdAt: Scalars['DateTime'];
  hostname: Scalars['String'];
  id: Scalars['UUID'];
  machineSpecificStamp: Scalars['String'];
};

export type GetPacketsByDeviceIdQueryInput = {
  dateFrom?: InputMaybe<Scalars['DateTime']>;
  dateTo?: InputMaybe<Scalars['DateTime']>;
  deviceId: Scalars['UUID'];
};

export type GetUserWithDevicesAndIpFiltersByIdQueryInput = {
  userId: Scalars['UUID'];
};

export enum IpFilterAction {
  Drop = 'DROP',
  PassWithoutCollecting = 'PASS_WITHOUT_COLLECTING'
}

export type IpFilterDto = {
  __typename?: 'IpFilterDto';
  comment?: Maybe<Scalars['String']>;
  createdAt: Scalars['DateTime'];
  filterAction: IpFilterAction;
  id: Scalars['UUID'];
  ip: Scalars['Long'];
};

export type LoginCommandInput = {
  hostname?: InputMaybe<Scalars['String']>;
  machineSpecificStamp?: InputMaybe<Scalars['String']>;
  password: Scalars['String'];
  username: Scalars['String'];
};

export type Mutation = {
  __typename?: 'Mutation';
  createIpFilter: Scalars['UUID'];
  deleteIpFilter: Scalars['Boolean'];
  login: TokenDto;
  refreshToken: TokenDto;
  register: Scalars['Boolean'];
  sendTwoFactorCode: Scalars['Boolean'];
  verifyTwoFactorCode: Scalars['Boolean'];
};


export type MutationCreateIpFilterArgs = {
  input: CreateIpFilterCommandInput;
};


export type MutationDeleteIpFilterArgs = {
  input: DeleteIpFilterCommandInput;
};


export type MutationLoginArgs = {
  input: LoginCommandInput;
};


export type MutationRefreshTokenArgs = {
  input: RefreshTokenCommandInput;
};


export type MutationRegisterArgs = {
  input: RegisterCommandInput;
};


export type MutationSendTwoFactorCodeArgs = {
  input: SendTwoFactorCodeCommandInput;
};


export type MutationVerifyTwoFactorCodeArgs = {
  input: VerifyTwoFactorCodeCommandInput;
};

export type PacketDto = {
  __typename?: 'PacketDto';
  createdAt: Scalars['DateTime'];
  id: Scalars['UUID'];
  ip: Scalars['Long'];
  protocol: ProtocolType;
  size: Scalars['Long'];
  status: PacketStatus;
};

export enum PacketStatus {
  Dropped = 'DROPPED',
  Passed = 'PASSED'
}

export enum ProtocolType {
  Ggp = 'GGP',
  Icmp = 'ICMP',
  IcmpV6 = 'ICMP_V6',
  Idp = 'IDP',
  Igmp = 'IGMP',
  Ipx = 'IPX',
  IpSecAuthenticationHeader = 'IP_SEC_AUTHENTICATION_HEADER',
  IpSecEncapsulatingSecurityPayload = 'IP_SEC_ENCAPSULATING_SECURITY_PAYLOAD',
  IPv4 = 'I_PV4',
  IPv6 = 'I_PV6',
  IPv6DestinationOptions = 'I_PV6_DESTINATION_OPTIONS',
  IPv6FragmentHeader = 'I_PV6_FRAGMENT_HEADER',
  IPv6HopByHopOptions = 'I_PV6_HOP_BY_HOP_OPTIONS',
  IPv6NoNextHeader = 'I_PV6_NO_NEXT_HEADER',
  IPv6RoutingHeader = 'I_PV6_ROUTING_HEADER',
  Nd = 'ND',
  Pup = 'PUP',
  Raw = 'RAW',
  Spx = 'SPX',
  SpxIi = 'SPX_II',
  Tcp = 'TCP',
  Udp = 'UDP',
  Unknown = 'UNKNOWN'
}

export type Query = {
  __typename?: 'Query';
  packetsByDeviceId: Array<PacketDto>;
  userById: UserDto;
};


export type QueryPacketsByDeviceIdArgs = {
  input: GetPacketsByDeviceIdQueryInput;
};


export type QueryUserByIdArgs = {
  input: GetUserWithDevicesAndIpFiltersByIdQueryInput;
};

export type RefreshTokenCommandInput = {
  accessToken: Scalars['String'];
  refreshToken: Scalars['String'];
};

export type RegisterCommandInput = {
  confirmPassword: Scalars['String'];
  email: Scalars['String'];
  password: Scalars['String'];
  username: Scalars['String'];
};

export type SendTwoFactorCodeCommandInput = {
  email: Scalars['String'];
};

export type TokenDto = {
  __typename?: 'TokenDto';
  accessToken: Scalars['String'];
  refreshToken: Scalars['String'];
};

export type UserDto = {
  __typename?: 'UserDto';
  devices: Array<DeviceDto>;
  email: Scalars['String'];
  id: Scalars['UUID'];
  ipFilters: Array<IpFilterDto>;
  username: Scalars['String'];
};

export type VerifyTwoFactorCodeCommandInput = {
  email: Scalars['String'];
  twoFactorCode: Scalars['String'];
};

export type LoginMutationVariables = Exact<{
  input: LoginCommandInput;
}>;


export type LoginMutation = { __typename?: 'Mutation', login: { __typename?: 'TokenDto', accessToken: string, refreshToken: string } };

export type RegisterMutationVariables = Exact<{
  input: RegisterCommandInput;
}>;


export type RegisterMutation = { __typename?: 'Mutation', register: boolean };

export type RefreshTokenMutationVariables = Exact<{
  input: RefreshTokenCommandInput;
}>;


export type RefreshTokenMutation = { __typename?: 'Mutation', refreshToken: { __typename?: 'TokenDto', accessToken: string, refreshToken: string } };

export type SendTwoFactorCodeMutationVariables = Exact<{
  input: SendTwoFactorCodeCommandInput;
}>;


export type SendTwoFactorCodeMutation = { __typename?: 'Mutation', sendTwoFactorCode: boolean };

export type VerifyTwoFactorCodeMutationVariables = Exact<{
  input: VerifyTwoFactorCodeCommandInput;
}>;


export type VerifyTwoFactorCodeMutation = { __typename?: 'Mutation', verifyTwoFactorCode: boolean };

export type CreateIpFilterMutationVariables = Exact<{
  input: CreateIpFilterCommandInput;
}>;


export type CreateIpFilterMutation = { __typename?: 'Mutation', createIpFilter: any };

export type DeleteIpFilterMutationVariables = Exact<{
  input: DeleteIpFilterCommandInput;
}>;


export type DeleteIpFilterMutation = { __typename?: 'Mutation', deleteIpFilter: boolean };

export type GetPacketsByDeviceIdQueryVariables = Exact<{
  input: GetPacketsByDeviceIdQueryInput;
}>;


export type GetPacketsByDeviceIdQuery = { __typename?: 'Query', packetsByDeviceId: Array<{ __typename?: 'PacketDto', id: any, ip: any, size: any, protocol: ProtocolType, status: PacketStatus, createdAt: any }> };

export type GetUserByIdQueryVariables = Exact<{
  input: GetUserWithDevicesAndIpFiltersByIdQueryInput;
}>;


export type GetUserByIdQuery = { __typename?: 'Query', userById: { __typename?: 'UserDto', devices: Array<{ __typename?: 'DeviceDto', id: any, hostname: string, machineSpecificStamp: string, createdAt: any }>, ipFilters: Array<{ __typename?: 'IpFilterDto', id: any, ip: any, filterAction: IpFilterAction, comment?: string | null, createdAt: any }> } };

export const LoginDocument = gql`
    mutation Login($input: LoginCommandInput!) {
  login(input: $input) {
    accessToken
    refreshToken
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class LoginGQL extends Apollo.Mutation<LoginMutation, LoginMutationVariables> {
    override document = LoginDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const RegisterDocument = gql`
    mutation Register($input: RegisterCommandInput!) {
  register(input: $input)
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class RegisterGQL extends Apollo.Mutation<RegisterMutation, RegisterMutationVariables> {
    override document = RegisterDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const RefreshTokenDocument = gql`
    mutation RefreshToken($input: RefreshTokenCommandInput!) {
  refreshToken(input: $input) {
    accessToken
    refreshToken
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class RefreshTokenGQL extends Apollo.Mutation<RefreshTokenMutation, RefreshTokenMutationVariables> {
    override document = RefreshTokenDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const SendTwoFactorCodeDocument = gql`
    mutation SendTwoFactorCode($input: SendTwoFactorCodeCommandInput!) {
  sendTwoFactorCode(input: $input)
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class SendTwoFactorCodeGQL extends Apollo.Mutation<SendTwoFactorCodeMutation, SendTwoFactorCodeMutationVariables> {
    override document = SendTwoFactorCodeDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const VerifyTwoFactorCodeDocument = gql`
    mutation VerifyTwoFactorCode($input: VerifyTwoFactorCodeCommandInput!) {
  verifyTwoFactorCode(input: $input)
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class VerifyTwoFactorCodeGQL extends Apollo.Mutation<VerifyTwoFactorCodeMutation, VerifyTwoFactorCodeMutationVariables> {
    override document = VerifyTwoFactorCodeDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const CreateIpFilterDocument = gql`
    mutation CreateIpFilter($input: CreateIpFilterCommandInput!) {
  createIpFilter(input: $input)
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class CreateIpFilterGQL extends Apollo.Mutation<CreateIpFilterMutation, CreateIpFilterMutationVariables> {
    override document = CreateIpFilterDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const DeleteIpFilterDocument = gql`
    mutation DeleteIpFilter($input: DeleteIpFilterCommandInput!) {
  deleteIpFilter(input: $input)
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class DeleteIpFilterGQL extends Apollo.Mutation<DeleteIpFilterMutation, DeleteIpFilterMutationVariables> {
    override document = DeleteIpFilterDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const GetPacketsByDeviceIdDocument = gql`
    query GetPacketsByDeviceId($input: GetPacketsByDeviceIdQueryInput!) {
  packetsByDeviceId(input: $input) {
    id
    ip
    size
    protocol
    status
    createdAt
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class GetPacketsByDeviceIdGQL extends Apollo.Query<GetPacketsByDeviceIdQuery, GetPacketsByDeviceIdQueryVariables> {
    override document = GetPacketsByDeviceIdDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const GetUserByIdDocument = gql`
    query GetUserById($input: GetUserWithDevicesAndIpFiltersByIdQueryInput!) {
  userById(input: $input) {
    devices {
      id
      hostname
      machineSpecificStamp
      createdAt
    }
    ipFilters {
      id
      ip
      filterAction
      comment
      createdAt
    }
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class GetUserByIdGQL extends Apollo.Query<GetUserByIdQuery, GetUserByIdQueryVariables> {
    override document = GetUserByIdDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
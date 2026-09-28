<div align="center">

# 🛰️ Network Monitoring Tool

**Kernel-level packet capture, real-time dashboards, and ML-powered DDoS mitigation.**

An eBPF/XDP agent watches every IPv4 packet on a network interface, streams metadata to a .NET backend over gRPC, and a Python service trains a neural network to spot hostile traffic and push block rules back to the kernel automatically.

[![Go](https://img.shields.io/badge/Go-1.19-00ADD8?logo=go&logoColor=white)](cli)
[![eBPF](https://img.shields.io/badge/eBPF-XDP-orange?logo=linux&logoColor=white)](cli/pkg/bpf)
[![.NET](https://img.shields.io/badge/.NET-7.0-512BD4?logo=dotnet&logoColor=white)](server)
[![Angular](https://img.shields.io/badge/Angular-15-DD0031?logo=angular&logoColor=white)](client)
[![Python](https://img.shields.io/badge/Python-3.10+-3776AB?logo=python&logoColor=white)](server/Nmt.DdosDetection)
[![gRPC](https://img.shields.io/badge/gRPC-Protobuf-244c5a?logo=google&logoColor=white)](server/Nmt.Grpc/Protos)
[![GraphQL](https://img.shields.io/badge/GraphQL-HotChocolate-E10098?logo=graphql&logoColor=white)](server/Nmt.GraphQL)

</div>

---

## ✨ Features

- **Zero-copy packet inspection** with an XDP program attached directly to the NIC driver hook. Filtering decisions happen before the kernel network stack ever sees the packet.
- **Per-IP filter rules** (`Drop`, `DropWithoutCollecting`, `PassWithoutCollecting`) stored in an LRU hash map inside the kernel and synced from the server on startup.
- **Live traffic dashboard** with per-device packet charts, day or week ranges, and full CRUD over IP filters.
- **Automatic DDoS response.** The backend detects traffic anomalies, hands the window to a scikit-learn MLP classifier, and hostile IPs are written back as `Drop` filters without human intervention.
- **Multi-device accounts.** One user can register many hosts, each fingerprinted with a machine-specific stamp.
- **Hardened auth.** JWT access and refresh tokens, email-verified registration, two-factor codes, and role-based permissions enforced on both gRPC and GraphQL.
- **CQRS with MediatR**, FluentValidation on every command, and a Redis-backed caching pipeline behaviour with event-driven invalidation.

---

### Component overview

| Component | Path | Stack | Role |
|---|---|---|---|
| **CLI agent** | [`cli/`](cli) | Go 1.19, cobra, cilium/ebpf, gRPC | Loads the XDP program, drains the kernel packet queue every few seconds, ships batches to the server. |
| **eBPF program** | [`cli/pkg/bpf/kernel/`](cli/pkg/bpf/kernel) | C, XDP | Parses Ethernet and IPv4 headers, applies per-IP filter actions, pushes packet metadata to a `BPF_MAP_TYPE_QUEUE`. |
| **gRPC API** | [`server/Nmt.Grpc`](server/Nmt.Grpc) | ASP.NET Core 7, Grpc.AspNetCore | Agent-facing API: login, token refresh, filter sync, packet ingestion. Consumes block events from RabbitMQ. |
| **GraphQL API** | [`server/Nmt.GraphQL`](server/Nmt.GraphQL) | HotChocolate 12 | Browser-facing API: auth, user info, chart data, IP filter mutations. |
| **Core** | [`server/Nmt.Core`](server/Nmt.Core) | MediatR, FluentValidation, MassTransit, Scrutor | CQRS commands and queries, pipeline behaviours, auth policies, bus consumers. |
| **Infrastructure** | [`server/Nmt.Infrastructure`](server/Nmt.Infrastructure) | EF Core + Npgsql, MongoDB.Driver, Redis | Data access, identity stores, migrations, caches. |
| **Domain** | [`server/Nmt.Domain`](server/Nmt.Domain) | Plain C# | Entities, enums, bus events, configs. |
| **DDoS detection** | [`server/Nmt.DdosDetection`](server/Nmt.DdosDetection) | Python, scikit-learn, pika, pymongoarrow | Trains and runs an MLP classifier over packet windows, publishes IPs to block. |
| **Web client** | [`client/`](client) | Angular 15, Angular Material, Apollo, ApexCharts | Login, registration, email verification, and the monitoring dashboard. |

---

## 🚀 Getting started

### Prerequisites

| Requirement | Used by |
|---|---|
| Linux kernel with XDP support, `clang`, `llvm`, `libbpf` headers | CLI agent |
| Go 1.19+, `protoc`, `protoc-gen-go`, `protoc-gen-go-grpc` | CLI agent |
| .NET 7 SDK | Server |
| Node.js 18+ and npm | Web client |
| Python 3.10+ | DDoS detection |
| PostgreSQL, MongoDB, Redis, RabbitMQ | Server and detection |

The default development connection strings live in [`server/Nmt.Grpc/appsettings.Development.json`](server/Nmt.Grpc/appsettings.Development.json) and expect all four services on `localhost` with default ports.

<div align="center">

</div>

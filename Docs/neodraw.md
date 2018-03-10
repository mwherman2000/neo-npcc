# NeoDraw - NEO Multi-Person Drawing dApp

**Reference dApp for the NEO Persistable Classes (NPC) Entity dApp (e-dApp) Platform v1.0**

`NeoDraw` is advanced proof-of-concept distributed application (dApp) for the NEO Persistable Classes (NPC) Entity dApp (e-dApp) Platform running on the NEO Blockchain. 

As a [`neo-csharpcoe`](https://github.com/mwherman2000/neo-csharpcoe/blob/master/README.md) compliant platform, the `NPC-e-dApp` platform includes a full suite of tools and libraries (code), frameworks, how-to documentation, and best practices for full-stack e-dApp development using .NET/C#, C#.NEO, and the NEO Blockchain:

| Component | Home Project |
| --------- | ------------ |
| NPC Framework | neo-persistableclasses |
| NPC Entity and Domain Model | neo-persistableclasses |
| NPC Structured Storage Architecture (Nested Storage Domains) | neo-persistableclasses |
| NPC NeoStorageKey Specificaton | neo-persistableclasses |
| NPC Entity Programming Model | neo-npcc |
| NPC C#.NPC Entity Programming Language | neo-npcc |
| NPC C#.NPC Compiler (npcc) | neo-npcc |
| NPC Source-level Execution Cost Profiler | merged into neo-debugger-tools |
| NPC Integrated Entity Tracing | neo-npcc |
| NPC Smart-formatting Event Log Views | merged into neo-gui-developer and neo-debugger-tools |
| NPC JSON Entity Deserialization | [merged into neo-lex](https://github.com/CityOfZion/neo-lux/pull/9) |

[NEO Blockchain C# Center of Excellence](https://github.com/mwherman2000/neo-csharpcoe/blob/master/README.md) ([`neo-csharpcoe`](https://github.com/mwherman2000/neo-csharpcoe/blob/master/README.md))

The `neo-csharpcoe` project is an "umbrella" project for several initiatives related to providing tools and libraries (code), frameworks, how-to documentation, and best practices for full-stack development using .NET/C#, C#.NEO and the NEO Blockchain.

The `neo-csharpcoe` is an independent, free, open source project that is 100% community-supported by people like yourself through your contributions of time, energy, passion, promotion, and donations. To learn more about contributing to the `neo-csharpcoe`, click [here](https://github.com/mwherman2000/neo-csharpcoe/blob/master/CONTRIBUTE.md).

## NPC Framework

This e-dApp was developed with the `neo-csharpcoe` [Neo Persistable Classes (NPC) Framework]((https://github.com/mwherman2000/neo-persistableclasses/blob/master/README.md)>) including full automatic code generation of the NEO Persistable Classes using the [NPC Compiler (npcc)](https://github.com/mwherman2000/neo-npcc/blob/master/README.md).

## NeoDraw Smart Contract Protocol

* `add    user  [encodedusername, encodedpassword]`
* `get    user  [encodedusername]`
* `add    point [encodedusername, x, y]`
* `getall point [encodedusername]`
* `delete point [encodedusername]`

## NeoDraw Client App Commands

* `add    point x y`
* `help`
* `exit`

## NeoDraw Client App

![NeoDraw](./images/NeoDraw0Color.png)


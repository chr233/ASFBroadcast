# ASFBroadcast

[![爱发电](https://img.shields.io/badge/爱发电-chr__-ea4aaa.svg?logo=github-sponsors)](https://afdian.net/@chr233)
[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

基于 [ASFEnhance](https://github.com/chr233/ASFEnhance) 开发

ASFEnhance 介绍 & 使用指南: [https://keylol.com/t804841-1-1](https://keylol.com/t804841-1-1)

## 下载链接

> 解压后将 "ASFBroadcast.dll" 丢进 ASF 目录下的 "plugins" 文件夹即可安装

[GitHub Releases](https://github.com/chr233/ASFBroadcast/releases)

## 适配说明

> 标 \* 代表理论上兼容但是未经测试, 如果 ASF 没有修改插件 API 理论上可以向后兼容

| ASFEnhance 版本                                            | 依赖 ASF 版本 | 5.2.4.2 | 5.2.5.7 | 5.2.6.3+ |
| ---------------------------------------------------------- | ------------- | ------- | ------- | -------- |
| [.3.x](https://github.com/chr233/ASFBroadcast/releases) | 5.2.6.3       | ❌      | ✔️      | ✔️\*     |

<details>
  <summary>历史版本</summary>

| ASFEnhance 版本 | 依赖 ASF 版本 | 5.1.2.5 | 5.2.2.5 | 5.2.3.7 | 5.2.4.2 | 5.2.5.6 |
| --------------- | ------------- | ------- | ------- | ------- | ------- | ------- |
| -               | -             | -       | -       | -       | -       | -       |

</details>

## 插件指令说明

### 其他命令

| 命令             | 缩写    | 权限            | 说明                     |
| ---------------- | ------- | --------------- | ------------------------ |
| `KEY <Text>`     | `K`     | `FamilySharing` | 从文本提取 key           |
| `ASFBroadcast`   | `ASFEX` | `FamilySharing` | 查看 ASFBroadcast 的版本 |
| `ASFEHELP`       | `EHELP` | `FamilySharing` | 查看全部指令说明         |
| `HELP <Command>` | -       | `FamilySharing` | 查看指令说明             |

### 社区相关

| 命令                           | 缩写  | 权限            | 说明                 |
| ------------------------------ | ----- | --------------- | -------------------- |
| `PROFILE [Bots]`               | `PF`  | `FamilySharing` | 查看个人资料         |
| `PROFILELINK [Bots]`           | `PFL` | `FamilySharing` | 查看个人资料链接     |
| `STEAMID [Bots]`               | `SID` | `FamilySharing` | 查看 steamID         |
| `FRIENDCODE [Bots]`            | `FC`  | `FamilySharing` | 查看好友代码         |
| `GROUPLIST [Bots]`             | `GL`  | `FamilySharing` | 查看机器人的群组列表 |
| `JOINGROUP [Bots] <GroupName>` | `JG`  | `Master`        | 加入指定群组         |
| `LEAVEGROUP [Bots] <GroupID>`  | `LG`  | `Master`        | 离开指定群组         |

> `GroupID`可以用命令`GROUPLIST`获取

### 愿望单相关

| 命令                             | 缩写  | 权限     | 说明                   |
| -------------------------------- | ----- | -------- | ---------------------- |
| `ADDWISHLIST [Bots] <AppIDs>`    | `AW`  | `Master` | 添加愿望单             |
| `REMOVEWISHLIST [Bots] <AppIDs>` | `RW`  | `Master` | 移除愿望单             |
| `FOLLOWGAME [Bots] <AppIDs>`     | `FG`  | `Master` | 关注游戏               |
| `UNFOLLOWGAME [Bots] <AppIDs>`   | `UFG` | `Master` | 取消关注游戏           |
| `CHECK [Bots] <AppIDs>`          | `CK`  | `Master` | 检查愿望单/关注/已拥有 |

### 商店相关

| 命令                                       | 缩写   | 权限       | 说明                                             |
| ------------------------------------------ | ------ | ---------- | ------------------------------------------------ |
| `APPDETAIL [Bots] <AppIDS>`                | `AD`   | `Operator` | 获取 APP 信息, 无法获取锁区游戏信息, 仅支持`APP` |
| `SEARCH [Bots] Keywords`                   | `SS`   | `Operator` | 搜索商店                                         |
| `SUBS [Bots] <AppIDS\|SubIDS\|BundleIDS>`  | `S`    | `Operator` | 查询商店 SUB, 支持`APP/SUB/BUNDLE`               |
| `PURCHASEHISTORY`                          | `PH`   | `Operator` | 读取商店消费历史记录                             |
| `PUBLISHRECOMMENT [Bots] <AppIDS> COMMENT` | `PREC` | `Master`   | 发布评测, APPID > 0 给好评, AppID < 0 给差评     |
| `DELETERECOMMENT [Bots] <AppIDS>`          | `DREC` | `Master`   | 删除评测 (有 BUG,暂不能正常工作)                 |

### 钱包相关

| 命令                          | 缩写  | 权限     | 说明           |
| ----------------------------- | ----- | -------- | -------------- |
| `REDEEMWALLET [Bots] <Codes>` | `RWA` | `Master` | 激活钱包充值码 |

### 购物车相关

> STEAM 的购物车储存在 Cookies 里,重启 ASF 将会导致购物车清空

| 命令                                 | 缩写  | 权限       | 说明                                                                |
| ------------------------------------ | ----- | ---------- | ------------------------------------------------------------------- |
| `CART [Bots]`                        | `C`   | `Operator` | 查看机器人购物车                                                    |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`  | `Operator` | 添加购物车, 仅能使用`SubID`和`BundleID`                             |
| `CARTRESET [Bots]`                   | `CR`  | `Operator` | 清空购物车                                                          |
| `CARTCOUNTRY [Bots]`                 | `CC`  | `Operator` | 获取购物车可用结算区域(跟账号钱包和当前 IP 所在地有关)              |
| `PURCHASE [Bots]`                    | `PC`  | `Owner`    | 结算机器人的购物车, 只能为机器人自己购买 (使用 Steam 钱包余额结算)  |
| `PURCHASEGIFT [BotA] BotB`           | `PCG` | `Owner`    | 结算机器人 A 的购物车, 发送礼物给机器人 B (使用 Steam 钱包余额结算) |

> Steam 允许重复购买,使用 `PURCHASE` 命令前请自行确认有无重复内容

### ASF 命令缩写

| 命令缩写               | 等价命令                       | 说明                       |
| ---------------------- | ------------------------------ | -------------------------- |
| `AL [Bots] <Licenses>` | `ADDLICENSE [Bots] <Licenses>` | 添加免费 SUB               |
| `LA`                   | `LEVEL ASF`                    | 获取所有机器人的等级       |
| `BA`                   | `BALANCE ASF`                  | 获取所有机器人的钱包余额   |
| `PA`                   | `POINTS ASF`                   | 获取所有机器人的点数余额   |
| `P [Bots]`             | `POINTS`                       | 获取机器人的点数余额       |
| `CA`                   | `CART ASF`                     | 获取所有机器人的购物车信息 |

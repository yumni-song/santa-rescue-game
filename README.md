# [Santa Rescue Game] 프로젝트 분석 및 README

![Unity C# Game Development](https://img.shields.io/badge/Unity-C%23-blue.svg?style=for-the-badge&logo=unity)
![GitHub Repo stars](https://img.shields.io/github/stars/yumni-song/santa-rescue-game?style=for-the-badge)
![GitHub last commit](https://img.shields.io/github/last-commit/yumni-song/santa-rescue-game?style=for-the-badge)

## 🚀 프로젝트 소개

이 프로젝트는 GitHub 저장소 `yumni-song/santa-rescue-game`에서 분석된 Unity 3D 게임 프로젝트입니다. 프로젝트명과 파일 구조, 스크립트 내용을 바탕으로 추정컨대, 플레이어가 산타를 구출하는 것을 목표로 하는 액션 어드벤처 또는 RPG 요소가 가미된 게임으로 보입니다.

본 README.md 문서는 프로젝트의 핵심 기능, 구조, 사용 기술 및 시스템 아키텍처를 상세하게 설명하여 면접 및 포트폴리오 제출에 적합하도록 작성되었습니다.

## ✨ 주요 기능

이 프로젝트는 플레이어와 적, 아이템 상호작용을 중심으로 한 다양한 핵심 게임 플레이 요소를 포함하고 있습니다.

*   **플레이어 시스템**:
    *   **이동 및 액션**: 걷기, 달리기, 점프 등 기본적인 플레이어 이동 로직 구현.
    *   **생명력 관리**: HP 관리, 데미지 적용, 회복, 무적 시간, 사망 처리(게임 오버 및 씬 전환) 기능.
    *   **시작 지점 이동**: 씬 로드 시 플레이어를 미리 지정된 시작 지점으로 이동.
*   **적 시스템**:
    *   **기본 속성**: 모든 적 캐릭터의 공통적인 HP, 데미지 속성 및 행동 정의.
    *   **사망 처리**: 적 사망 시 이펙트 및 사운드 재생.
    *   **플레이어 보호막 상호작용**: 플레이어 보호막과의 충돌 처리 로직.
*   **아이템 및 인벤토리 시스템**:
    *   **아이템 획득/관리**: 필드 아이템 획득 및 인벤토리에 보관, 제거 기능.
    *   **아이템 스폰**: 게임 씬 내에 아이템을 고정 또는 랜덤으로 스폰하는 데이터베이스 역할.
*   **사용자 인터페이스 (UI)**:
    *   **인벤토리 UI**: 인벤토리 슬롯 시각화, 마우스 스크롤 아이템 선택, 클릭 사용 기능.
    *   **플레이어 HP UI**: `PlayerHealth` 상태를 하트 아이콘으로 표시, 피격 시 하트 깜빡임 피드백.
    *   **타이머 UI**: 게임 제한 시간을 표시하고 업데이트.
*   **핵심 게임 시스템**:
    *   **시간 관리**: 게임 내 제한 시간 관리, 시간 초과 시 'BadEnd' 씬으로 전환.
    *   **씬 관리**: 시작, 인게임, 엔딩 씬 등 주요 씬 간의 전환을 중앙에서 제어.
    *   **오디오 관리**: 배경 음악(BGM) 및 효과음(SFX)을 전역적으로 관리하며 씬 전환 시 BGM 제어.

## 🏗️ 프로젝트 구조

프로젝트는 Unity Engine의 표준적인 에셋 관리 방식을 따르며, 스크립트들은 역할별로 `Assets/Scripts` 하위에 명확하게 분리되어 있습니다.

*   **`Assets/Scenes`**: 게임의 각 단계(시작, 튜토리얼, 인게임, 엔딩 등)를 구성하는 Unity 씬 파일들을 포함합니다.
*   **`Assets/Prefabs`**: 재사용 가능한 게임 오브젝트 템플릿들을 보관합니다. (예: 플레이어, 적, 아이템 프리팹)
*   **`Assets/Scripts`**: 게임 로직을 구현하는 C# 스크립트들을 기능별로 분류하여 관리합니다.
    *   **`Assets/Scripts/Player`**: 플레이어의 이동, 체력, 인벤토리 등 플레이어 관련 로직.
    *   **`Assets/Scripts/Enemy`**: 적 캐릭터의 공통 로직 및 개별 적(보스 포함) 로직.
    *   **`Assets/Scripts/Item`**: 아이템의 데이터베이스, 인벤토리 관리, 필드 아이템 상호작용 로직.
    *   **`Assets/Scripts/UI`**: 게임 내 UI(HP, 인벤토리, 타이머 등) 관련 로직.
    *   **`Assets/Scripts/Scene`**: 씬 전환 및 게임 흐름 제어 로직.
    *   **`Assets/Scripts/Audio`**: 오디오 관리 로직.
    *   **`Assets/Scripts/Managers`**: 전역적으로 사용되는 매니저 스크립트(예: TimeManager, AudioManager).
*   **`Assets/Models`, `Assets/Textures`, `Assets/Materials`, `Assets/Audio`**: 3D 모델, 텍스처, 재질, 오디오 클립 등 게임 리소스들을 보관합니다.

(참고: 정확한 디렉토리 구조 정보가 제공되지 않아, 일반적인 Unity 프로젝트 구조 및 핵심 파일 경로를 기반으로 추정하여 작성되었습니다.)

## 📝 핵심 파일 설명

프로젝트의 주요 로직을 담당하는 핵심 스크립트 파일들은 다음과 같습니다.

*   **`Assets/Scripts/Player/PlayerMovement.cs`**: 플레이어 캐릭터의 핵심 이동(걷기, 달리기, 점프) 로직을 구현하며, 사망 시 캐릭터 움직임을 정지시키는 기능을 포함합니다.
*   **`Assets/Scripts/Player/PlayerHealth.cs`**: 플레이어의 생명력을 관리합니다. 데미지 적용, HP 회복, 피격 무적 시간, 사망 처리(게임 오버 및 씬 전환) 기능을 담당하며, 씬 로드 시 플레이어를 시작 지점으로 이동시킵니다.
*   **`Assets/Scripts/Enemy/EnemyBase.cs`**: 모든 적 캐릭터의 공통적인 속성(HP, 데미지)과 동작, 사망 시 처리(이펙트, 사운드)를 정의하는 기반 스크립트입니다. 플레이어 보호막과의 상호작용 로직이 포함되어 있습니다.
*   **`Assets/Scripts/Item/Inventory.cs`**: 플레이어가 획득한 아이템을 보관하고 관리하는 인벤토리 시스템의 핵심 로직을 구현합니다. 아이템 추가/제거 및 필드 아이템과의 상호작용을 처리합니다.
*   **`Assets/Scripts/Item/ItemDatabase.cs`**: 게임에 등장하는 아이템들의 데이터베이스 역할을 하며, 게임 씬 내에 아이템을 스폰하는 로직을 담당합니다. 고정 스폰 및 랜덤 스폰 기능을 제공합니다.
*   **`Assets/Scripts/UI/InventoryUI.cs`**: 인벤토리 시스템의 사용자 인터페이스를 제어합니다. 인벤토리 슬롯을 시각적으로 표시하고, 마우스 스크롤로 아이템을 선택하고 클릭으로 사용하는 기능을 구현합니다.
*   **`Assets/Scripts/UI/PlayerHealthUI.cs`**: `PlayerHealth`의 현재 HP 상태를 받아 화면에 하트 아이콘으로 표시합니다. 플레이어가 데미지를 입었을 때 하트가 깜빡이는 시각적 피드백을 제공합니다.
*   **`Assets/Scripts/TimeManager.cs`**: 게임 내 제한 시간을 관리하고, 타이머 UI를 업데이트합니다. 시간이 0이 되면 'BadEnd' 씬으로 전환하여 게임 실패 조건을 구현합니다.
*   **`Assets/Scripts/Scene/SceneController.cs`**: 게임의 시작, 인게임, 엔딩 씬 등 주요 씬 간의 전환을 담당하는 중앙 제어 스크립트입니다.
*   **`Assets/Scripts/AudioManager.cs`**: 게임의 배경 음악(BGM) 및 효과음 재생을 전역적으로 관리합니다. 씬 전환 시 BGM을 제어하는 기능을 포함합니다.

## 🛠️ 기술 스택

프로젝트는 Unity Engine을 기반으로 한 3D 게임 개발에 필요한 다양한 기술과 도구를 활용합니다.

### Game Client / Frontend
*   **Unity Engine**: 통합된 개발 환경과 강력한 툴셋을 활용하여 효율적인 3D 게임 개발 및 멀티 플랫폼 배포가 가능합니다.
*   **C#**: Unity의 주력 스크립팅 언어로, 객체 지향 프로그래밍을 통해 게임 로직을 체계적이고 확장 가능하게 구현할 수 있습니다.
*   **Unity UI System**: 게임 내 사용자 인터페이스(HP 바, 인벤토리, 타이머)를 직관적으로 설계하고 관리할 수 있습니다.
*   **TextMesh Pro**: 고품질의 텍스트 렌더링을 제공하여 게임 내 텍스트 UI의 시각적 품질과 성능을 향상시킵니다.
*   **3D 모델 (.fbx)**: 다양한 캐릭터와 환경 오브젝트를 활용하여 풍부하고 몰입감 있는 게임 세계를 구축할 수 있습니다.
*   **이미지/스프라이트 (.png)**: UI 요소, 텍스처, 이펙트 등 다양한 2D/3D 그래픽 리소스를 유연하게 통합할 수 있습니다.
*   **오디오 (.wav, .mp3)**: 배경 음악과 효과음을 통해 게임의 분위기를 조성하고 플레이어에게 다채로운 피드백을 제공하여 몰입도를 높입니다.
*   **Prefabs**: 재사용 가능한 게임 오브젝트 템플릿을 통해 일관성 있는 컴포넌트 구성을 유지하고 개발 시간을 단축합니다.
*   **Scenes (.unity)**: 게임의 각 단계(메인 메뉴, 인게임 레벨, 엔딩)를 모듈화하여 관리함으로써 개발 및 테스트 효율성을 높입니다.

### Database
*   **ScriptableObject / In-Editor Data Management**: 'ItemDatabase.cs'와 같이 Unity 에디터 내에서 데이터셋(아이템 목록, 스폰 위치)을 스크립트화된 오브젝트로 관리하여 빠르고 직관적인 콘텐츠 설계를 가능하게 합니다.

### DevOps
*   **Git**: 분산 버전 관리 시스템을 사용하여 코드 및 에셋 변경 이력을 효율적으로 추적하고 협업 개발의 안정성을 확보합니다.

## 🏛️ 시스템 아키텍처

이 프로젝트는 Unity Engine을 기반으로 한 단일 플레이어 게임 클라이언트 아키텍처를 가지고 있습니다.

게임의 핵심 로직은 C# 스크립트로 구현되며, `PlayerHealth`, `Inventory`, `TimerManager`, `AudioManager`와 같은 주요 기능들은 `DontDestroyOnLoad` 패턴을 사용하여 씬 전환에도 지속되는 싱글톤(Singleton) 형태로 전역 상태를 관리합니다. 이는 게임의 핵심 데이터와 매니저들이 씬 로드와 관계없이 일관성을 유지하고, 필요한 다른 스크립트에서 쉽게 접근하여 사용할 수 있도록 설계되었음을 의미합니다.

게임 세계는 여러 Unity 씬으로 구성되며, 각 씬은 플레이어, 적, 아이템 등의 게임 오브젝트 인스턴스를 포함합니다. UI 시스템은 TextMesh Pro와 Unity UI를 활용하여 플레이어의 HP, 인벤토리, 타이머 등을 직관적으로 표시합니다. 전체적으로 클라이언트 측에서 모든 게임 로직과 데이터 처리가 이루어지는 독립 실행형 게임 형태입니다.

```mermaid
graph TD
    subgraph "Game Client Application"
        A[("Unity Engine")]:::engine -- orchestrates --> B["Persistent Game State (Singletons)"]:::core_logic
        A -- renders & interacts with --> K["Game Scenes (.unity)"]:::game_content
        A -- provides resources to --> L["Game Assets (Prefabs, Models, UI, Audio)"]:::assets

        B -- "includes & manages" --> B1["PlayerHealth Singleton"]:::core_logic
        B -- "includes & manages" --> B2["Inventory Singleton"]:::core_logic
        B -- "includes & manages" --> B3["TimerManager Singleton"]:::core_logic
        B -- "includes & manages" --> B4["AudioManager Singleton"]:::core_logic

        K -- hosts --> C["Game Logic Modules"]:::game_logic
        K -- uses --> L

        C -- "comprises" --> C1["Player Logic (Movement, Health, Actions)"]:::game_logic
        C -- "comprises" --> C2["Enemy Logic (Spawning, AI, Combat)"]:::game_logic
        C -- "comprises" --> C3["Item Logic (Spawning, Usage)"]:::game_logic
        C -- "comprises" --> C4["UI Logic (HUD, Inventory UI)"]:::game_logic
        C -- "comprises" --> C5["Scene Flow Control"]:::game_logic

        C1 -- updates -.-> B1
        C1 -- interacts with --> C2
        C1 -- interacts with --> C3
        C1 -- updates -.-> C4
        C2 -- interacts with --> C1
        C2 -- uses --> L
        C3 -- updates -.-> B2
        C3 -- interacts with --> C1
        C3 -- uses --> L
        C4 -- displays -.-> B1
        C4 -- displays -.-> B2
        C4 -- displays -.-> B3
        C4 -- uses --> L
        C5 -- loads -.-> K
        C5 -- triggers -.-> B3 : (GameOver)
        C5 -- notifies -.-> B4 : (Scene Change)

        B1 -.-> C4 : Updates UI
        B1 -.-> C5 : Triggers End Game
        B2 -.-> C4 : Updates UI
        B3 -.-> C4 : Updates UI
        B3 -.-> C5 : Triggers End Game (Time Out)
        B4 -.-> L : Plays Audio Clips
        B4 -.-> C5 : Responds to Scene Change

        L -- provides models/sprites/sounds to --> C1, C2, C3, C4
    end

    classDef engine fill:#FFD700,stroke:#333,stroke-width:2px; /* Gold */
    classDef core_logic fill:#4682B4,stroke:#333,stroke-width:2px; /* SteelBlue */
    classDef game_logic fill:#98FB98,stroke:#333,stroke-width:2px; /* PaleGreen */
    classDef game_content fill:#DDA0DD,stroke:#333,stroke-width:2px; /* Plum */
    classDef assets fill:#D2B48C,stroke:#333,stroke-width:2px; /* Tan */
```

## ▶️ 실행 방법

프로젝트 실행에 대한 구체적인 빌드 및 실행 지침은 현재 정보에 없어 **추가 작성 필요**합니다.

일반적인 Unity 프로젝트 실행 방법은 다음과 같습니다:

1.  **Unity Hub 설치**: Unity Hub와 Unity Editor (프로젝트가 생성된 버전 또는 호환 가능한 버전)를 설치합니다.
2.  **프로젝트 클론**: GitHub 저장소를 로컬 환경으로 클론합니다.
    ```bash
    git clone https://github.com/yumni-song/santa-rescue-game.git
    ```
3.  **Unity Editor 열기**: Unity Hub에서 '프로젝트 열기'를 통해 클론한 프로젝트 폴더를 선택하여 Unity Editor에서 엽니다.
4.  **씬 실행**: `Assets/Scenes/StartScene.unity` 또는 메인 게임 플레이 씬을 열고 Unity Editor의 플레이 버튼(▶)을 눌러 게임을 실행합니다.

## 🎯 기술 선택 이유

프로젝트에서 사용된 주요 기술 스택의 선택 이유는 다음과 같습니다.

*   **Unity Engine**: 통합된 개발 환경과 강력한 툴셋을 제공하여 효율적인 3D 게임 개발 및 다양한 플랫폼으로의 배포를 가능하게 합니다.
*   **C#**: Unity의 주력 스크립팅 언어로서, 객체 지향 프로그래밍 패러다임을 통해 게임 로직을 체계적이고 확장 가능하도록 구현하는 데 용이합니다.
*   **Unity UI System & TextMesh Pro**: 직관적인 인터페이스 설계와 관리를 돕고, 고품질의 텍스트 렌더링을 통해 게임 UI의 시각적 품질과 성능을 동시에 향상시킵니다.
*   **ScriptableObject / In-Editor Data Management**: Unity 에디터 내에서 데이터셋을 스크립트화된 오브젝트로 직접 관리하여 빠르고 직관적인 콘텐츠 제작 및 데이터 설계를 가능하게 합니다.
*   **Git**: 분산 버전 관리 시스템으로, 코드와 에셋 변경 이력을 효율적으로 추적하고 개발 과정에서의 협업 안정성을 보장합니다.

## 🚀 개선 방향

현재 프로젝트의 분석 결과를 바탕으로 다음과 같은 개선 방향을 고려할 수 있습니다.

*   **게임 목표 및 스토리 명확화**: (추정) 'santa-rescue-game'이라는 이름에서 산타 구출이 주 목표임은 알 수 있으나, 구체적인 스토리 라인, 서브 퀘스트, 세계관 설정 등을 README 및 게임 내에 명확히 제시하여 플레이어의 몰입도를 높일 수 있습니다.
*   **'Random' 아이템 효과 구체화**: (추정) `FieldItem_Random.prefab`을 통해 랜덤 아이템 스폰 기능이 존재하지만, 인게임에서 이 아이템이 어떤 구체적인 효과(예: 무작위 버프/디버프 부여, 특정 아이템 변환 등)를 가지는지 명확히 정의하고 시각적/청각적 피드백을 추가하여 플레이 경험을 풍부하게 할 수 있습니다.
*   **`PlayerAcquirer.cs` 상세 기능 정의**: (추정) 플레이어가 어떤 종류의 오브젝트를 '획득'하는지 명확한 내용이 없으므로, 아이템 외에 다른 상호작용 가능한 오브젝트(예: 미션 아이템, 퍼즐 조각) 획득 로직이라면 이를 확장하거나 기능을 명확히 문서화할 수 있습니다.
*   **보스 클래스 상속 구조 정리**: (추정) `GhostBoss.cs`와 `WitchBoss.cs`가 `EnemyBase`를 상속받는 것은 확인되지만, `BossEnemyBase.cs`와의 직접적인 상속 관계가 명확하지 않습니다. 보스 캐릭터만을 위한 공통 기능을 `BossEnemyBase`에 정의하고 이를 상속받도록 하여 코드의 재사용성과 일관성을 높일 수 있습니다.
*   **레벨 디자인 및 콘텐츠 확장**: 현재 씬 구성 외에 더 다양한 레벨, 적 유형, 아이템, 퍼즐 요소를 추가하여 게임 플레이 시간을 늘리고 재미를 더할 수 있습니다.
*   **성능 최적화**: 대규모 씬이나 복잡한 로직에서 발생할 수 있는 프레임 드랍을 방지하기 위해 렌더링 최적화(LOD, Occlusion Culling), 스크립트 최적화 등을 적용할 수 있습니다.
*   **에러 핸들링 및 견고성 강화**: 예외 처리 로직을 추가하여 예상치 못한 상황에서도 게임이 안정적으로 동작하도록 견고성을 높일 수 있습니다. (예: NullReferenceException 방지, 입력 유효성 검사)
*   **사용자 경험(UX) 개선**: UI/UX 전문가의 피드백을 받아 인벤토리 관리, HP 표시, 튜토리얼 안내 등을 더욱 직관적이고 편리하게 개선할 수 있습니다.
*   **저장 및 불러오기 기능**: 플레이어가 게임 진행 상황을 저장하고 나중에 다시 불러올 수 있는 기능을 추가하여 플레이 편의성을 높일 수 있습니다.
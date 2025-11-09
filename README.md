### 깃허브 협업 설정 과정

0. 3d 게임 프로젝트를 유니티에서 생성
1. GameProject라는 원격 레포지토리 생성
2. $ C:/Users/ippun/GameProject 폴더로 이동
3. $ git init
4. $ git remote add origin https://github.com/yumni-song/game-project
4.5. Unity 에디터 설정Edit > Project Settings > Editor
     Version Control: Visible Meta Files
     Asset Serialization: Force Text
     메타(.meta) 파일 생성 + 씬/프리팹을 YAML 텍스트로 저장해 병합 가능하게 함.
5. $ git lfs install
6. $ git lfs track "*.fbx" "*.psd" "*.wav" "*.mp3" "*.mp4" "*.tga" "*.exr" "*.zip"
7. .gitignore, .gitattributes 파일 만들기
8. $ git add .
9. $ git commit -m "init: base Unity project setup"
10. $ git branch -M main
11. $ git push -u origin main

### 프로젝트 받기
```bash
git clone https://github.com/yumni-song/game-project
cd game-project
git lfs install
git lfs pull   # (선택) 대용량 에셋 강제 가져오기
```

### 유니티로 열기
 - Unity Hub → Add project from disk → game-project 폴더 선택 → 같은 버전으로 Open
 - (프로젝트에 이미 Visible Meta Files / Force Text가 저장돼 있어서 그대로 따라옵니다)

### 작업 루틴(브랜치 방식)
```bash
git pull                                # 시작 전 최신화
git checkout -b feature/<작업이름>      # 개인 브랜치 생성
# Unity에서 작업…
git add .
git commit -m "feat: <무엇을 했는지 요약>"
git push -u origin feature/<작업이름>   # 원격에 올리기
```

### 머지 후 로컬 정리
```bash
git switch main
git pull
git branch -d feature/<작업이름>
git push origin --delete feature/<작업이름>
```

### .gitignore
```gitignore
# === Unity 기본 무시 목록 ===
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]serSettings/
[Mm]emoryCaptures/
[Oo]bj/
[Bb]in/
[.]*.swp

# === IDE 관련 ===
.vscode/
.idea/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs

# === OS 관련 ===
.DS_Store
Thumbs.db

# === Crash Dumps ===
sysinfo.txt

# === Rider / Visual Studio Cache ===
*.pidb
*.booproj

# === Package Cache ===
Packages/com.unity.collab-proxy/
Packages/com.unity.package-manager-ui/

# === 빌드 결과물 ===
Build/
Builds/

# === 반드시 추적해야 하는 폴더 ===
!Assets/
!ProjectSettings/
!Packages/
```

### gitattributes
```gitattributes
# Unity Large Binary Assets
*.psd filter=lfs diff=lfs merge=lfs -text
*.tga filter=lfs diff=lfs merge=lfs -text
*.png filter=lfs diff=lfs merge=lfs -text
*.jpg filter=lfs diff=lfs merge=lfs -text
*.fbx filter=lfs diff=lfs merge=lfs -text
*.obj filter=lfs diff=lfs merge=lfs -text
*.wav filter=lfs diff=lfs merge=lfs -text
*.mp3 filter=lfs diff=lfs merge=lfs -text
*.mp4 filter=lfs diff=lfs merge=lfs -text
*.mov filter=lfs diff=lfs merge=lfs -text
*.zip filter=lfs diff=lfs merge=lfs -text
*.exr filter=lfs diff=lfs merge=lfs -text
```
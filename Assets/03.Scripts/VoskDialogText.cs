using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class VoskDialogText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;
    public Text DialogText;

    // 인사 명령어
    Regex hi_regex = new Regex(@"(안녕하세요|여보세요)");

// "누구야" 명령어
    Regex who_regex = new Regex(@"(누구야|너는 누구야|네 이름은?)");

// "좋아"나 "시작하자" 등의 긍정적 반응
    Regex pass_regex = new Regex(@"(좋아|네|시작하자|좋습니다)");

// "도와줘" 명령어
    Regex help_regex = new Regex(@"(도와줘|도와주세요|도와줄 수 있어?)");

// 염소 관련 명령어
    Regex goat_regex = new Regex(@"(염소|시작은 염소로|염소부터 시작)");

// 늑대 관련 명령어
    Regex wolf_regex = new Regex(@"(늑대|늑대를)");

// 배추 관련 명령어
    Regex cabbage_regex = new Regex(@"(배추|배추를|배추부터 시작)");

// 염소를 뒤로 보내는 명령어
    Regex goat_back_regex = new Regex(@"(염소 뒤로|염소를 되돌려|염소를 뒤로 보내)");

// 늑대를 뒤로 보내는 명령어
    Regex wolf_back_regex = new Regex(@"(늑대 뒤로|늑대를 되돌려|늑대를 뒤로 보내)");

// 배추를 뒤로 보내는 명령어
    Regex cabbage_back_regex = new Regex(@"(배추 뒤로|배추를 되돌려|배추를 뒤로 보내)");

// "앞으로 가자" 명령어
    Regex forward_regex = new Regex(@"(앞으로 가|앞으로 이동|전진)");

// "뒤로 가자" 명령어
    Regex back_regex = new Regex(@"(뒤로 가|뒤로 이동|되돌아가자|뒤로 가기)");


	// State
	bool goat_left;
	bool wolf_left;
	bool cabbage_left;
	bool man_left;

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
		ResetState();
    }

	void ResetState()
	{
		goat_left = true;
		wolf_left = true;
		cabbage_left = true;
		man_left = true;
	}

    void CheckState() {
        // 염소, 늑대, 농부 상태 체크
        if (goat_left && wolf_left && !man_left) {
            AddFinalResponse("늑대가 염소를 잡아먹었어요. 처음부터 다시 시작하세요.");
            return;
        }
        if (goat_left && cabbage_left && !man_left) {
            AddFinalResponse("염소가 배추를 먹었어요. 처음부터 다시 시작하세요.");
            return;
        }
        if (!goat_left && !wolf_left && man_left) {
            AddFinalResponse("늑대가 염소를 잡아먹었어요. 처음부터 다시 시작하세요.");
            return;
        }
        if (!goat_left && !cabbage_left && man_left) {
            AddFinalResponse("염소가 배추를 먹었어요. 처음부터 다시 시작하세요.");
            return;
        }

        // 모두 오른쪽에 있을 때 성공 메시지
        if (!goat_left && !wolf_left && !cabbage_left && !man_left) {
            AddFinalResponse("모두 안전하게 건넜어요! 다시 시도해볼까요?");
            return;
        }

        // 기본적인 안내 메시지
        AddResponse("그럼, 이제 뭐 할까요?");
    }


	void Say(string response)
	{ 
        // macOS에서 텍스트를 음성으로 변환하여 출력하는 코드
        // System.Diagnostics.Process.Start("/usr/bin/say", response);
        
        // window
        Debug.Log(response);
        
	}

	void AddFinalResponse(string response) {
		Say(response);
		DialogText.text = response + "\n";
		ResetState();
	}

    void AddResponse(string response) {
        Say(response);  // 음성 응답

        DialogText.text = response + "\n\n";

        // 농부, 늑대, 염소, 배추의 위치를 한국어로 표시
        DialogText.text += "농부 " + (man_left ? "왼쪽" : "오른쪽") + "\n";
        DialogText.text += "늑대 " + (wolf_left ? "왼쪽" : "오른쪽") + "\n";
        DialogText.text += "염소 " + (goat_left ? "왼쪽" : "오른쪽") + "\n";
        DialogText.text += "배추 " + (cabbage_left ? "왼쪽" : "오른쪽") + "\n";

        DialogText.text += "\n";
    }


    private void OnTranscriptionResult(string obj)
    {
        // Save to file

        Debug.Log(obj);
        var result = new RecognitionResult(obj);
        foreach (RecognizedPhrase p in result.Phrases)
        {
            if (hi_regex.IsMatch(p.Text))
            {
                AddResponse("안녕하세요!");
                return;
            }
            if (who_regex.IsMatch(p.Text))
            {
                AddResponse("저는 로봇 선생님이에요.");
                return;
            }
            if (pass_regex.IsMatch(p.Text))
            {
                AddResponse("좋아요!");
                return;
            }
            if (help_regex.IsMatch(p.Text))
            {
                AddResponse("스스로 생각해보세요.");
                return;
            }
            if (goat_back_regex.IsMatch(p.Text)) {
                if (goat_left == true) {
                    AddResponse("염소는 아직 왼쪽에 있어요.");
                } else if (man_left == true) {
                    AddResponse("농부는 아직 왼쪽에 있어요.");
                } else {
                    goat_left = true;
                    man_left = true;
                    CheckState();
                }
                return;
            }

            if (wolf_back_regex.IsMatch(p.Text)) {
                if (wolf_left == true) {
                    AddResponse("늑대는 아직 왼쪽에 있어요.");
                } else if (man_left == true) {
                    AddResponse("농부는 아직 왼쪽에 있어요.");
                } else {
                    wolf_left = true;
                    man_left = true;
                    CheckState();
                }
                return;
            }

            if (cabbage_back_regex.IsMatch(p.Text)) {
                if (cabbage_left == true) {
                    AddResponse("배추는 아직 왼쪽에 있어요.");
                } else if (man_left == true) {
                    AddResponse("농부는 아직 왼쪽에 있어요.");
                } else {
                    cabbage_left = true;
                    man_left = true;
                    CheckState();
                }
                return;
            }

            if (goat_regex.IsMatch(p.Text)) {
                if (goat_left == false) {
                    AddResponse("염소는 이미 오른쪽에 있어요.");
                } else if (man_left == false) {
                    AddResponse("농부는 이미 오른쪽에 있어요.");
                } else {
                    goat_left = false;
                    man_left = false;
                    CheckState();
                }
                return;
            }

            if (wolf_regex.IsMatch(p.Text)) {
                if (wolf_left == false) {
                    AddResponse("늑대는 이미 오른쪽에 있어요.");
                } else if (man_left == false) {
                    AddResponse("농부는 이미 오른쪽에 있어요.");
                } else {
                    wolf_left = false;
                    man_left = false;
                    CheckState();
                }
                return;
            }

            if (cabbage_regex.IsMatch(p.Text)) {
                if (cabbage_left == false) {
                    AddResponse("배추는 이미 오른쪽에 있어요.");
                } else if (man_left == false) {
                    AddResponse("농부는 이미 오른쪽에 있어요.");
                } else {
                    cabbage_left = false;
                    man_left = false;
                    CheckState();
                }
                return;
            }

            if (forward_regex.IsMatch(p.Text)) {
                if (man_left == false) {
                    AddResponse("농부는 이미 오른쪽에 있어요.");
                } else {
                    man_left = false;
                    CheckState();
                }
                return;
            }
        
            if (back_regex.IsMatch(p.Text)) {
                if (man_left == true) {
                    AddResponse("농부는 아직 왼쪽에 있어요.");
                } else {
                    man_left = true;
                    CheckState();
                }
                return;
            }
        }
        if (result.Phrases.Length > 0 && result.Phrases[0].Text != "") {
            AddResponse("이해하지 못했어요.");
        }
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NozoCharacterController : MonoBehaviour
{
    #region MovementVariable
    [SerializeField] private float WalkSpeed;          // Tốc độ đi bộ
    [SerializeField] private float RunSpeed;           // Tốc độ chạy
    [SerializeField] private float Gravity;           // Lực trọng trường
    [SerializeField] private Transform Player_Charac; // Transform của Player

    private CharacterController characterController;  // Component điều khiển chuyển động
    private Vector3 direction;                       // Hướng di chuyển
    private float moveSpeed;                        // Tốc độ di chuyển hiện tại
    private Vector3 velocity;                       // Vectơ vận tốc
    private float horizontal;                      // Giá trị trục ngang (từ input)

    [SerializeField] private bool isGrounded;         // Biến kiểm tra có đang chạm đất không
    [SerializeField] private float GroundCheckDistance; // Khoảng cách kiểm tra chạm đất
    [SerializeField] LayerMask GroundMask;         // LayerMask để kiểm tra va chạm chạm đất

    private Animator ani;                        // Component Animator (điều khiển animation)

    [SerializeField] private AnimationCurve dodgeCurve; // Đường cong animation cho hành động né

    public bool isDodging = false;        // Biến kiểm tra có đang né không
    private bool Dodge_Exe = false;       // Biến kiểm tra hành động né có đang thực thi không
    private float dodgeTimer;               // Biến lưu thời gian né
    public static bool WasInteractPressed = false; // Biến static kiểm tra phím tương tác có được nhấn không

    private UIManager uiManager;        // Tham chiếu đến UIManager
    private bool isSelectWindowShow = false;  // Biến kiểm tra cửa sổ lựa chọn có đang hiển thị không
    private PlayerStats playerStats;         // Tham chiếu đến PlayerStats (lưu trữ chỉ số người chơi)
    private int rollStaminaCost = 40;         // Chi phí stamina khi né
    private int runStaminaCost = 20;         // Chi phí stamina khi chạy

    public AudioManager audioManager; // Tham chiếu đến AudioManager
    #endregion
    private void Awake()
    {
        // Tìm kiếm GameObject với tag "Audio" và lấy AudioManager component từ nó.
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        GameObject tempChar = GameObject.FindGameObjectWithTag("Player"); // Tìm kiếm GameObject với tag "Player".
        characterController = tempChar.GetComponent<CharacterController>(); // Lấy CharacterController component từ GameObject được tìm thấy.
        ani = tempChar.GetComponentInChildren<Animator>(); //  Lấy Animator component từ con (hoặc chính GameObject) được tìm thấy.
        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1]; // Lấy keyframe cuối cùng của dodgeCurve để tính toán thời gian né tránh.
        dodgeTimer = dodge_lastFrame.time; // Thiết lập thời gian né tránh dựa trên keyframe cuối cùng.
        playerStats = tempChar.GetComponent<PlayerStats>(); // Lấy PlayerStats component từ GameObject được tìm thấy
                                                            // (chú ý: cần đảm bảo PlayerStats component được đính kèm).
        uiManager = FindObjectOfType<UIManager>(); // Tìm kiếm UIManager component trên tất cả các GameObject trong cảnh.
    }
    void Update()
    {
        Gravity_Control(); // Kiểm tra xem nhân vật có đang đứng trên mặt đất hay không và áp dụng trọng lực.
        if (!WasInteractPressed)
        {
            // Điều kiện này đảm bảo:

            // Nhân vật không đang né tránh.
            // Nhân vật không đang thực hiện tấn công.
            // Nhân vật không đang bị Kẻ thù tấn công.
            // Nhân vật không đang chết.
            if (!isDodging && !ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack") &&
                !ani.GetCurrentAnimatorStateInfo(0).IsTag("Impact") && !GetComponent<Death>().isDeath)
            {
                Player_Rota(); // Xoay nhân vật theo hướng di chuyển.
                Dichuyen();
            }

            // Kiểm tra: Nhấn phím K, đang không thực hiện né tránh, nhân vật chưa chết
            if (Input.GetKeyDown(KeyCode.K) && !Dodge_Exe && !GetComponent<Death>().isDeath)
            {
                // Nhân vật đang di chuyển (direction != Vector3.zero).
                // Nhân vật có đủ Stamina để né tránh (playerStats.currentStamina > rollStaminaCost).
                if (direction.magnitude != 0 && playerStats.currentStamina > rollStaminaCost)
                {
                    StartCoroutine(Dodge()); 
                    playerStats.TakeStaminaDamage(rollStaminaCost); // Trừ Stamina tương ứng với StaminaCost.
                };
                return;
            }
            else if(Input.GetKeyDown(KeyCode.K) && Input.GetKeyDown(KeyCode.LeftShift) && !Dodge_Exe && !GetComponent<Death>().isDeath)
            {
                // Kiểm tra:
                // Nhấn phím K và Shift cùng lúc.
                // Nhân vật có đủ Stamina để né tránh (playerStats.currentStamina > rollStaminaCost + 10).
                if (direction.magnitude != 0 && playerStats.currentStamina > rollStaminaCost)
                {
                    StartCoroutine(Dodge());
                    playerStats.TakeStaminaDamage(rollStaminaCost + 10); //
                };
                return;
            }

            // Lấy
            // Mở cửa sổ lựa chọn khi nhấn phím Escape và cửa sổ chưa hiển thị
            if (Input.GetKeyDown(KeyCode.Escape) && !isSelectWindowShow)
            {
                uiManager.OpenSelectWindow();
                isSelectWindowShow = true; // Đánh dấu rằng cửa sổ đã được mở
            }
            // Đóng cửa sổ lựa chọn khi nhấn phím Escape và cửa sổ đã hiển thị
            else if (Input.GetKeyDown(KeyCode.Escape) && isSelectWindowShow)
            {
                uiManager.CloseSelectWindow();
                isSelectWindowShow = false; // Đánh dấu rằng cửa sổ đã được đóng
            }
        }
    }

    private void Dichuyen()
    {

        horizontal = Input.GetAxisRaw("Horizontal"); // Return Float (Khi ấn A hoặc D)
        var goc_quay = characterController.transform.eulerAngles.y; // Lấy góc quay hiện tại của nhân vật.
        if (goc_quay == 90f) // khi góc xoay của nhân vật là 90 độ
        {
            direction = new Vector3(horizontal, 0, 0); // Cho nó đi theo trục x
        }    
        else if(goc_quay == 0) // // khi góc xoay của nhân vật là 0 độ
        {
            direction = new Vector3(0, 0, horizontal); // Cho nó đi theo trục z
        }

        if (!isGrounded)
        {
            ani.SetBool("IsMoving", true); // Chuyển Animation sang Blend Tree Move
            // Kiểm tra:
            // Nhân vật đang di chuyển(direction != Vector3.zero).
            // Nhấn phím Shift.
            if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                // Khi người chơi muốn đi bộ
                Walk();
            }
            else if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                // Khi người chơi muốn chạy
                // Nhân vật có đủ Stamina để chạy (playerStats.currentStamina > runStaminaCost).
                if (playerStats.currentStamina > runStaminaCost)
                {
                    Run();
                    playerStats.TakeStaminaDamage(runStaminaCost);
                }
                else
                {
                    // Nếu nhân vật không đủ Stamina để chạy
                    Walk();
                }
            }  
            else if(direction == Vector3.zero)
            {
                ani.SetBool("IsMoving", false); // Dừng Animation trong Blend Tree
            }    
            direction *= moveSpeed; // Nhân hướng di chuyển với tốc độ di chuyển hiện tại.
        }
        characterController.Move(direction * Time.deltaTime);
        
    }
    private void Player_Rota()
    {
       Vector3 current_Rota = Player_Charac.transform.localEulerAngles; //  Lấy góc quay cục bộ của Transform "Player_Charac"
        switch (horizontal)
        {
            case 1:
            case 2:
                Mv_forward(current_Rota);
                break;
            case -1:
            case -2:
                Mv_backward(current_Rota);
                break;
        }
    }     
    private void Mv_forward(Vector3 current_Rota)
    {
        if(characterController.transform.position.y != 0) // Khi nhân vật đang đi về phía sau
        {
            current_Rota.y = 0f; // Chuyển hướng Player_Charac thành quay về phía trước
            Player_Charac.transform.localEulerAngles = current_Rota; // thay đổi góc xoay về phía trước
        }    
    }   
    
    private void Mv_backward(Vector3 current_Rota)
    {
        if (characterController.transform.position.y != -180) // Khi nhân vật đang đi về phía trước
        {
            current_Rota.y = -180f; // // Chuyển hướng Player_Charac thành quay về phía trước
            Player_Charac.transform.localEulerAngles = current_Rota; // thành đổi góc xoay về phái sau
        }
    }
    // Kiểm tra xem nhân vật có đang đứng trên mặt đất không
    private void Gravity_Control()
    {
        // Hàm Physics.CheckSphere kiểm tra xem có va chạm nào trong hình cầu xung quanh
        // vị trí hiện tại của nhân vật (transform.position)
        // với bán kính GroundCheckDistance
        // và các lớp được xác định bởi GroundMask.
        isGrounded = Physics.CheckSphere(transform.position, GroundCheckDistance, GroundMask);
        if (!isGrounded && velocity.y < 0)
        {
            // Nếu nhân vật không còn trên mặt đất và vận tốc theo trục Y âm (đang rơi)
            velocity.y = -2f;
        }
        // Giảm vận tốc theo trục Y theo gia tốc trọng lực (Gravity)
        // và khoảng thời gian đã trôi qua (Time.deltaTime).
        velocity.y -= Gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        // Sử dụng characterController.Move để di chuyển nhân vật theo vận tốc
        // đã tính toán (velocity * Time.deltaTime).
    }
    private void Walk()
    {
        // Đặt tốc độ di chuyển thành tốc độ đi bộ (moveSpeed = WalkSpeed)
        moveSpeed = WalkSpeed;
        // Sử dụng ani.SetFloat để thiết lập giá trị cho tham số Ani_Transition trong animation.
        // Giá trị 0 tương ứng với trạng thái đi bộ trong animation controller.
        ani.SetFloat("Ani_Transition", 0, 0.1f, Time.deltaTime);
    }    

    private void Run()
    {
        // Đặt tốc độ di chuyển thành tốc độ chạy(moveSpeed = RunSpeed)
        moveSpeed = RunSpeed;
        // Sử dụng ani.SetFloat để thiết lập giá trị cho tham số Ani_Transition trong animation.
        // Giá trị 1 tương ứng với trạng thái chạy trong animation controller.
        ani.SetFloat("Ani_Transition", 1f, 0.1f, Time.deltaTime);
    }   
    IEnumerator Dodge()
    {
        // Sử dụng ani.SetTrigger để kích hoạt trigger "Dodge" trong animation controller.
        // Sử dụng ani.SetBool để đặt cờ "IsMoving" thành false,
        // có thể để báo hiệu nhân vật đang né tránh và không thể di chuyển bình thường.
        ani.SetTrigger("Dodge");
        ani.SetBool("IsMoving", false);
        // Đặt isDodging thành true để theo dõi trạng thái né tránh của nhân vật.
        // Đặt Dodge_Exe thành true có thể để kiểm soát việc thực hiện né tránh.
        isDodging = true;
        Dodge_Exe = true;
        float timer = 0;
        while(timer <dodgeTimer)
        {
            // Tính toán tốc độ né tránh theo đường cong dodgeCurve và thời gian timer.
            float speed = dodgeCurve.Evaluate(timer);
            // Tạo hướng di chuyển né tránh (dir) dựa trên hướng được thiết lập (direction) và tốc độ đã tính.
            Vector3 dir = (direction * speed);
            // Di chuyển nhân vật theo hướng né tránh với tốc độ đã tính
            // Cập nhật timer theo khoảng thời gian đã trôi qua (Time.deltaTime).
            characterController.Move(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        // Đặt isDodging thành false để báo hiệu né tránh đã hoàn thành.
        // Đặt Dodge_Exe thành false có thể để dừng kiểm soát việc thực hiện né tránh.
        isDodging = false;
        Dodge_Exe = false;
    }
}

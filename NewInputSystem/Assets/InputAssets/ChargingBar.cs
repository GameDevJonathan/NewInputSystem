using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChargingBar : MonoBehaviour
{
    //get a reference to the slider 
    [SerializeField]
    private Slider _slider;

    //create a reference to the input system
    private PlayerInputActions _actions;

    //create a flag to show if we're charging or not.
    private bool _isCharging = false;

    //create a designer friendly variable to dictate the charging speed
    [SerializeField]
    private float _chargingSpeed = 3.0f;

    private void Start()
    {
        //intialize the input action and enable the action map
        _actions = new PlayerInputActions();
        _actions.ChargeBar.Enable();

        //set the slider value to 0 so we can start with an empty bar
        _slider.value = 0f;

        //register the started and cancled functions 
        _actions.ChargeBar.Charging.started += Charging_started;
        _actions.ChargeBar.Charging.canceled += Charging_canceled;
    }
    private void Charging_started(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //1 - so the logic is to start a coroutine to start charging the bar when the isCharging bool/flag is true
        //first set the bool to true and then start the couroutine.
        _isCharging = true;

        StartCoroutine(ChargingSliderBar());

        
    }

    private void Charging_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //set is charging to false to begin decreasing
        _isCharging =false;
    }

    
    //2 create ienumarator to handle logic
    IEnumerator ChargingSliderBar()
    {
        //the logic we are going to use in a while loop that will do what's in the code block WHILE the argument is true
        //in this case _isCharging.
        
        while(_isCharging) 
        {
            //note if you write out a varible that is a bool and with out the checking it's value ie: == true || == false
            //the arguement will automatically check it's absolute value.. is this case the argument is "true" if the value is true
            // opposite case !bang will evaluate to TRUE if the value is False.

            //let's increase the slider value
            _slider.value += (1 * Time.deltaTime) / _chargingSpeed;

            //since we're using an IEnumerator we need to yeild the program and return something. in this case we will 
            //pause and return nothing which will continue at the end of the frame.
            yield return  null;

            //note on infinite loop, if you create a loop that doesn't break your computer will crash because of all the available memory being used to run the loop
            //please be very careful
        }

        //create logic that decreases the slider value when not charging
        while(_slider.value > 0f)
        {
            _slider.value -= 1f * Time.deltaTime;
            yield return null;  
        }

    }

}

// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using System;

public class GameTimer {
    private float _timer;
    private bool _eventSent;
    private bool _sendEvent;
    private static int _interval;
    public event Action TimerEvent;

    /// <param name="interval">How often to send out event</param>
    public GameTimer(int interval) => _interval = interval;

    public void Start() {
        _timer = 0;
        _sendEvent = false;
        _eventSent = false;
    }

    public void Tick(float gameTick) {
        _timer += gameTick;
        if (_timer > 1) {
            _sendEvent = (int)_timer % _interval == 0;

            // to make sure the event is sent once
            if (_sendEvent && !_eventSent) {
                TimerEvent?.Invoke();
                _eventSent = true;
            }

            if (!_sendEvent) _eventSent = false;
        }
    }
}
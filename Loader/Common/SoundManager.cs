using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public static class SoundManager
    {
        private static SoundEffect _song;
        private static SoundEffect _songMuffled;
        private static SoundEffectInstance _songInstance;
        private static SoundEffectInstance _songMuffledInstance;

        private static SoundEffect _buttonUp;
        private static SoundEffect _buttonDown;
        private static SoundEffectInstance _buttonUpInstance;
        private static SoundEffectInstance _buttonDownInstance;

        private static SoundEffect _jumpSound;
        private static SoundEffectInstance _jumpSoundInstance;

        private static SoundEffect _mouseClick;
        private static List<SoundEffectInstance> _clickInstances = new();
        private static int _clickIndex = 0;

        private static SoundEffect _mouseClickUp;
        private static List<SoundEffectInstance> _clickUpInstances = new();
        private static int _upIndex = 0;

        private static SoundEffect _mouseClickDown;
        private static List<SoundEffectInstance> _clickDownInstances = new();
        private static int _downIndex = 0;

        public static void LoadContent(ContentManager content)
        {
            _song = content.Load<SoundEffect>("sounds/song");
            _songInstance = _song.CreateInstance();
            _songInstance.Volume = 0f;
            _songInstance.IsLooped = true;
            _songMuffled = content.Load<SoundEffect>("sounds/song_muff");
            _songMuffledInstance = _songMuffled.CreateInstance();
            _songMuffledInstance.Volume = 0f;
            _songMuffledInstance.IsLooped = true;
            _songInstance.Play();
            _songMuffledInstance.Play();

            _jumpSound = content.Load<SoundEffect>("sounds/jump");
            _jumpSoundInstance = _jumpSound.CreateInstance();
            _jumpSoundInstance.Volume = 0.25f;

            _buttonUp = content.Load<SoundEffect>("sounds/button_up");
            _buttonUpInstance = _buttonUp.CreateInstance();
            _buttonUpInstance.Volume = 0.5f;
            _buttonDown = content.Load<SoundEffect>("sounds/button_down");
            _buttonDownInstance = _buttonDown.CreateInstance();
            _buttonDownInstance.Volume = 0.5f;

            _mouseClick = content.Load<SoundEffect>("sounds/mouse_click");
            for (int i = 0; i < 10; i++)
            {
                var instance = _mouseClick.CreateInstance();
                instance.Volume = 0.6f;
                _clickInstances.Add(instance);
            }

            _mouseClickUp = content.Load<SoundEffect>("sounds/mouse_click_up");
            for(int i = 0; i < 10; i++)
            {
                var instance = _mouseClickUp.CreateInstance();
                instance.Volume = 0.6f;
                _clickUpInstances.Add(instance);
            }

            _mouseClickDown = content.Load<SoundEffect>("sounds/mouse_click_down");
            for (int i = 0; i < 10; i++)
            {
                var instance = _mouseClickDown.CreateInstance();
                instance.Volume = 0.6f;
                _clickDownInstances.Add(instance);
            }
        }

        public static void PlayMouseClickFull()
        {
            _clickInstances[_clickIndex].Play();
            _clickIndex = (_clickIndex + 1) % 10;
        }

        public static void PlayJumpSound()
        {
            _jumpSoundInstance.Play();
        }

        public static void PlayButtonDown()
        {
            _buttonDownInstance.Play();
        }

        public static void PlayButtonUp()
        {
            _buttonUpInstance.Play();
        }

        public static void PlaySong()
        {
            _songMuffledInstance.Volume = 0f;
            _songInstance.Volume = .4f;
        }

        public static void PlaySongMuffled()
        {
            _songInstance.Volume = 0f;
            _songMuffledInstance.Volume = 1f;
        }

        public static void PlayMouseClickUp()
        {
            _clickUpInstances[_upIndex].Play();
            _upIndex = (_upIndex + 1) % 10;
        }

        public static void PlayMouseClickDown()
        {
            _clickDownInstances[_downIndex].Play();
            _downIndex = (_downIndex + 1) % 10;
        }
    }
}

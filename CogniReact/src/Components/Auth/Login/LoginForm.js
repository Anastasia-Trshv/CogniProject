import React, { useContext, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ReactComponent as LogoSvg } from './img/logo.svg';
import './LoginForm.css';
import { Context } from '../../..';
import { observer } from 'mobx-react-lite';

function LoginForm() {

  const navigate = useNavigate();

  const {store} = useContext(Context);

  const [user, setUser] = React.useState({
    email: '',
    password: '',
  });

  const login = async (user) => {
    var response = await store.login(user);
    if(response) {
      navigate('/profile');
    }
	};

  const onSubmit = (e) => {
    e.preventDefault();
    login(user);
  }

  return (
    <div className='login__wrapper'>
        <div className='login__blur'>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
        </div>

        <form onSubmit={onSubmit} className='login__form loginform'>
            <h1 className='loginform__header'>Вход в «COGNI»</h1>
            <input
                    value={user?.email}
                    onChange={(e) => setUser({ ...user, email: e.target.value })}
                    type="text" placeholder='Введите e-mail' className='loginform__input'/>
                {/* {(validEmail) && <span className='loginform__error'><InfoIcon></InfoIcon><p>Email указан не верно</p></span>} */}

                <input
                    value={user?.password}
                    onChange={(e) => setUser({ ...user, password: e.target.value })}
                    type="password" placeholder='Придумайте пароль' className='loginform__input'/>
                {/* {(validPassword) && <span className='loginform__error'><InfoIcon></InfoIcon><p>Пароль должен состоять минимум из 8 символов и содержать хотя бы одну цифру/букву и один из символов: !@#$%^&*</p></span>} */}

            <button type="submit" className='loginform__button'>Войти</button>
            <Link to="/register" className='loginform__link'>Зарегистрироваться</Link>
        </form>
       
        <div className='login__info info'>
            <LogoSvg className='info__logo'/>
            <p className='info__logoText'>COGNI</p>
            <a href="" className='info__link'>О сервисе</a>
        </div>
    </div>
  );
};

export default observer(LoginForm);


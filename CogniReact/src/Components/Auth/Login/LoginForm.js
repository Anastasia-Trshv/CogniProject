import React from 'react';
import { Link } from 'react-router-dom';
import { ReactComponent as LogoSvg } from './img/logo.svg';
import './LoginForm.css';

function LoginForm() {

  return (
    <div className='login__wrapper'>
        <div className='login__blur'>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
        </div>

        <form action="" className='login__form loginform'>
            <h1 className='loginform__header'>Вход в «COGNI»</h1>
            <input type="text" name="login" placeholder='Введите email' className='loginform__input'/>
            <input type="password" name="password" placeholder='Введите пароль' className='loginform__input'/>
            <Link to="/profile" type="button" className='loginform__button'>Войти</Link>
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

export default LoginForm;


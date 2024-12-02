import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ReactComponent as LogoSvg } from './img/logo.svg';
import { ReactComponent as InfoIcon } from './img/info-icon.svg';
import { isFormValid, isFormMbtiValid } from "../../../services/auth.js";
import {observer} from "mobx-react-lite";
import {Context} from "../../../index";
import './RegisterForm.css';


function RegisterForm() {

    const navigate = useNavigate();

    const {store} = useContext(Context);

    //2 шага регистрации
    const [formStep, setFormStep] = React.useState(0);

    const completeFormStep = () => {
        let validStatus = isFormValid(user, passwordRepeat);
        setValidName(false);
        setValidSurname(false);
        setValidEmail(false);
        setValidPassword(false);
        setValidPasswordRepeat(false);

        if (validStatus.status) {
            setFormStep(cur => cur + 1);
        } else {
            
            if (validStatus.name) {
                setValidName(true);
            }
            if (validStatus.surname) {
                setValidSurname(true);
            }
            if (validStatus.email) {
                setValidEmail(true);
            }
            if (validStatus.password) {
                setValidPassword(true);
            }
            if (validStatus.passwordRepeat) {
                setValidPasswordRepeat(true);
            }
        }
    };

    //Создание пользователя
    const onCreate = async (user) => {
        var response = await store.register(user);
        if(response) {
         navigate('/profile');
        }
	};

    //При отправке данных вызывается onCreate
    const onSubmit = (e) => {
        console.log(12345);
        e.preventDefault();
        let validStatus = isFormMbtiValid(user);
        setValidMBTI(false);
        if (validStatus.status) {
            console.log(user);
            onCreate(user);
        } else {
            if(validStatus.mbti) {
                setValidMBTI(true);
            }
        }
    }

    const onTest = (e) => {
        localStorage.setItem('name', user.name);
        localStorage.setItem('surname', user.surname);
        localStorage.setItem('email', user.email);
        localStorage.setItem('password', user.password);
        localStorage.setItem('mbtiType', user.mbtiType);
        navigate('/test');
    }

    const [user, setUser] = React.useState({
        name: '',
        surname: '',
        email: '',
        password: '',
        mbtiType: '',
    });

    const [passwordRepeat, setPasswordRepeat] = React.useState('');

    const [validName, setValidName] = React.useState(false);
    const [validSurname, setValidSurname] = React.useState(false);
    const [validEmail, setValidEmail] = React.useState(false);
    const [validPassword, setValidPassword] = React.useState(false);
    const [validPasswordRepeat, setValidPasswordRepeat] = React.useState(false);
    const [validMBTI, setValidMBTI] = React.useState(false);


  return (
    <div className='login__wrapper'>
        <div className='login__blur'>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
            <div className='login__ball'></div>
        </div>
        
        <form onSubmit={onSubmit} className='login__form loginform'>
            {formStep === 0 &&  (<section>
                <h1 className='loginform__header'>Регистрация</h1>
                <input 
                    value={user?.name}
                    onChange={(e) => setUser({ ...user, name: e.target.value })}
                    type="text" placeholder='Имя' className='loginform__input'/>
                {(validName) && <span className='loginform__error'><InfoIcon></InfoIcon><p>Имя должно начинаться с заглавной буквы, быть написано на русском и содержать минимум 2 символа</p></span>}

                <input 
                    value={user?.surname}
                    onChange={(e) => setUser({ ...user, surname: e.target.value })}
                    type="text" placeholder='Фамилия' className='loginform__input'/>
                {(validSurname) && <span className='loginform__error'><InfoIcon></InfoIcon><p>Фамилия должна начинаться с заглавной буквы, быть написано на русском и содержать минимум 2 символа</p></span>}

                <input
                    value={user?.email}
                    onChange={(e) => setUser({ ...user, email: e.target.value })}
                    type="text" placeholder='Введите e-mail' className='loginform__input'/>
                {(validEmail) && <span className='loginform__error'><InfoIcon></InfoIcon><p>Email указан не верно</p></span>}

                <input
                    value={user?.password}
                    onChange={(e) => setUser({ ...user, password: e.target.value })}
                    type="password" placeholder='Придумайте пароль' className='loginform__input'/>
                {(validPassword) && <span className='loginform__error'><InfoIcon></InfoIcon><p>Пароль должен состоять минимум из 8 символов и содержать хотя бы одну цифру/букву и один из символов: !@#$%^&*</p></span>}

                <input
                onChange={(e) => setPasswordRepeat(e.target.value)}
                type="password" placeholder='Подтвердите пароль' className='loginform__input'/>
                {(validPasswordRepeat) && <span className='loginform__error'><InfoIcon></InfoIcon><p>Пароли не совпадают</p></span>}

                {/*<label htmlFor="agreeRadio" className='loginform__label'><input type="checkbox" className='loginform__radio' id="agreeRadio"/><p>Согласен на обработку персональных данных <a href="" className='loginform__agree'>Пользовательское соглашение</a></p></label>*/}
                <button onClick={completeFormStep} type="button" className='loginform__button'>Продолжить</button>
                <p className='loginform__desc'>Уже есть аккаунт?</p>
                <Link to="/login" className='loginform__link color-green'>Войти</Link>
            </section>
            )}

            {formStep === 1 &&  (<section>
                <h1 className='loginform__header'>Введите свой тип <br/> личности</h1>
                <input
                value={user?.mbtiId}
                onChange={(e) => setUser({ ...user, mbtiType: e.target.value })}
                type="text" placeholder='Ваш MBTI' className='loginform__input'/>
                {(validMBTI) && <span className='loginform__error'><InfoIcon></InfoIcon><p>MBTI введен не верно</p></span>}

                <button type="submit" className='loginform__button'>Зарегистрироваться</button>
                <p className='loginform__text'>или</p>
                <button onClick={onTest} type="button" className='loginform__button-other'>Пройти тест <br/> и узнать свой тип</button>
            </section>
            )}
        </form>
       
        <div className='login__info info'>
            <LogoSvg className='info__logo'/>
            <p className='info__logoText'>COGNI</p>
            <a href="#" className='info__link'>О сервисе</a>
        </div>
    </div>
  );
};

export default observer(RegisterForm);

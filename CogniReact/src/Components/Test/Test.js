import React, { useContext, useEffect } from 'react';
import './Test.css';
import axios from 'axios';
import { Link, useNavigate } from 'react-router-dom';
import { Context } from '../..';
import Header from '../Layouts/Header/Header';

function Test() {

    const [questions, setQuestions] = React.useState([]);

    const navigate = useNavigate();

    const {store} = useContext(Context);

    const [user, setUser] = React.useState({
        name: '',
        surname: '',
        email: '',
        password: '',
        mbtiType: 'INFP',
    });

    //Создание пользователя
    const onCreate = async (user) => {
        var response = await store.register(user);
        if(response) {
         navigate('/profile');
        }
	};

    //При отправке данных вызывается onCreate
    const onSubmitTest = () => {
        setUser((user) => ({ ...user, name: localStorage.getItem('name')}));
        setUser((user) => ({ ...user, surname: localStorage.getItem('surname')}));
        setUser((user) => ({ ...user, email: localStorage.getItem('email')}));
        setUser((user) => ({ ...user, password: localStorage.getItem('password')}));
        if (user.password == localStorage.getItem('password')) {
            onCreate(user);
        }
    };

    useEffect(() => {
        axios.get("https://localhost:7055/Test/GetAllQuestions")
            .then((response) => {
                setQuestions(response.data.questions);
             })
             .catch((err) => {
                console.log(err);
             }); 
      }, [])



    return (
        <div className='test'>
            <Header></Header>
            <div className="test__bg">
            <div className="test__wrapper">
            <ul>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[0]?.question}
                    </p>
                    <label><input name="question1" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question1" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question1" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question1" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[1]?.question}
                    </p>
                    <label><input name="question2" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question2" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question2" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question2" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[2]?.question}
                    </p>
                    <label><input name="question3" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question3" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question3" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question3" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[3]?.question}
                    </p>
                    <label><input name="question4" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question4" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question4" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question4" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[4]?.question}
                    </p>
                    <label><input name="question5" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question5" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question5" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question5" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[5]?.question}
                    </p>
                    <label><input name="question6" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question6" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question6" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question6" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[6]?.question}
                    </p>
                    <label><input name="question7" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question7" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question7" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question7" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[7]?.question}
                    </p>
                    <label><input name="question8" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question8" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question8" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question8" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[8]?.question}
                    </p>
                    <label><input name="question9" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question9" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question9" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question9" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[9]?.question}
                    </p>
                    <label><input name="question10" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question10" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question10" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question10" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[10]?.question}
                    </p>
                    <label><input name="question11" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question11" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question11" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question11" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[11]?.question}
                    </p>
                    <label><input name="question12" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question12" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question12" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question12" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[12]?.question}
                    </p>
                    <label><input name="question13" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question13" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question13" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question13" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[13]?.question}
                    </p>
                    <label><input name="question14" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question14" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question14" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question14" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[14]?.question}
                    </p>
                    <label><input name="question15" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question15" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question15" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question15" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[15]?.question}
                    </p>
                    <label><input name="question16" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question16" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question16" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question16" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[16]?.question}
                    </p>
                    <label><input name="question17" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question17" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question17" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question17" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[17]?.question}
                    </p>
                    <label><input name="question18" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question18" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question18" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question18" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[18]?.question}
                    </p>
                    <label><input name="question19" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question19" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question19" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question19" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[19]?.question}
                    </p>
                    <label><input name="question20" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question20" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question20" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question20" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[20]?.question}
                    </p>
                    <label><input name="question21" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question21" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question21" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question21" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[21]?.question}
                    </p>
                    <label><input name="question22" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question22" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question22" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question22" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[22]?.question}
                    </p>
                    <label><input name="question23" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question23" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question23" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question23" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[23]?.question}
                    </p>
                    <label><input name="question24" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question24" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question24" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question24" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[24]?.question}
                    </p>
                    <label><input name="question25" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question25" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question25" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question25" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[25]?.question}
                    </p>
                    <label><input name="question26" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question26" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question26" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question26" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[26]?.question}
                    </p>
                    <label><input name="question27" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question27" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question27" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question27" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[27]?.question}
                    </p>
                    <label><input name="question28" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question28" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question28" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question28" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[28]?.question}
                    </p>
                    <label><input name="question29" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question29" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question29" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question29" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[29]?.question}
                    </p>
                    <label><input name="question30" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question30" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question30" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question30" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[30]?.question}
                    </p>
                    <label><input name="question31" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question31" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question31" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question31" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[31]?.question}
                    </p>
                    <label><input name="question32" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question32" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question32" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question32" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[32]?.question}
                    </p>
                    <label><input name="question33" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question33" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question33" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question33" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[33]?.question}
                    </p>
                    <label><input name="question34" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question34" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question34" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question34" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[34]?.question}
                    </p>
                    <label><input name="question35" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question35" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question35" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question35" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[35]?.question}
                    </p>
                    <label><input name="question36" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question36" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question36" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question36" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[36]?.question}
                    </p>
                    <label><input name="question37" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question37" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question37" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question37" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[37]?.question}
                    </p>
                    <label><input name="question38" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question38" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question38" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question38" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[38]?.question}
                    </p>
                    <label><input name="question39" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question39" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question39" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question39" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[39]?.question}
                    </p>
                    <label><input name="question40" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question40" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question40" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question40" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[40]?.question}
                    </p>
                    <label><input name="question41" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question41" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question41" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question41" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[41]?.question}
                    </p>
                    <label><input name="question42" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question42" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question42" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question42" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[42]?.question}
                    </p>
                    <label><input name="question43" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question43" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question43" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question43" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[43]?.question}
                    </p>
                    <label><input name="question44" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question44" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question44" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question44" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[44]?.question}
                    </p>
                    <label><input name="question45" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question45" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question45" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question45" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[45]?.question}
                    </p>
                    <label><input name="question46" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question46" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question46" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question46" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[46]?.question}
                    </p>
                    <label><input name="question47" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question47" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question47" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question47" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[47]?.question}
                    </p>
                    <label><input name="question48" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question48" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question48" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question48" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[48]?.question}
                    </p>
                    <label><input name="question49" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question49" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question49" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question49" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[49]?.question}
                    </p>
                    <label><input name="question50" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question50" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question50" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question50" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[50]?.question}
                    </p>
                    <label><input name="question51" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question51" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question51" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question51" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[51]?.question}
                    </p>
                    <label><input name="question52" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question52" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question52" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question52" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[52]?.question}
                    </p>
                    <label><input name="question53" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question53" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question53" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question53" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[53]?.question}
                    </p>
                    <label><input name="question54" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question54" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question54" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question54" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[54]?.question}
                    </p>
                    <label><input name="question55" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question55" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question55" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question55" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[55]?.question}
                    </p>
                    <label><input name="question56" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question56" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question56" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question56" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[56]?.question}
                    </p>
                    <label><input name="question57" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question57" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question57" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question57" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[57]?.question}
                    </p>
                    <label><input name="question58" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question58" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question58" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question58" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[58]?.question}
                    </p>
                    <label><input name="question59" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question59" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question59" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question59" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
                <li className='test__item'>
                    <p className='test__question'>
                    {Object.values(questions)[59]?.question}
                    </p>
                    <label><input name="question60" className='test__input' type="radio"></input><span className='test__answer'>Нет</span></label>
                    <label><input name="question60" className='test__input' type="radio"></input><span className='test__answer'>Скорее нет, чем да</span></label>
                    <label><input name="question60" className='test__input' type="radio"></input><span className='test__answer'>Скорее да, чем нет</span></label>
                    <label><input name="question60" className='test__input' type="radio"></input><span className='test__answer'>Да</span></label>
                </li>
            </ul>
            <button onClick={onSubmitTest} className='test__button'>Завершить тест</button>
            </div>
            </div>
        </div>
      );
    };
    
export default Test;
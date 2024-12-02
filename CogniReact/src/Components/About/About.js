import React from "react";
import './About.css';
import { ReactComponent as LogoMbtiSvg } from './img/logo-mbti.svg';
import { ReactComponent as StarSvg } from './img/star.svg';
import { ReactComponent as StarsSvg } from './img/stars.svg';
import { ReactComponent as StafillSvg } from './img/star-fill.svg';
import { ReactComponent as TextSvg } from './img/text.svg';
import { ReactComponent as Text2Svg } from './img/text2.svg';
import { ReactComponent as WaweSvg } from './img/wawe.svg';
import { ReactComponent as Wawe2Svg } from './img/wawe2.svg';
import { ReactComponent as CogniSvg } from './img/COGNI.svg';
import { ReactComponent as TypesSvg } from './img/types.svg';
import { ReactComponent as BorderSvg } from './img/border.svg';
import { ReactComponent as Border2Svg } from './img/border2.svg';
import image1 from './img/team/image1.png';
import image2 from './img/team/image2.png';
import image3 from './img/team/image3.png';
import image4 from './img/team/image4.png';
import image5 from './img/team/image5.png';
import image6 from './img/team/image6.png';
import image7 from './img/team/image7.png';
import image8 from './img/team/image8.png';
import image9 from './img/team/image9.png';
import image10 from './img/team/image10.png';
import image11 from './img/team/image11.png';
import image12 from './img/team/image12.png';
import image13 from './img/team/image13.png';
import image14 from './img/team/image14.png';
import image15 from './img/team/image15.png';
import image16 from './img/team/image16.png';
import image17 from './img/team/image17.png';
import image18 from './img/team/image18.png';
import image19 from './img/team/image19.png';
import image20 from './img/team/image20.png';
import image21 from './img/team/image21.png';
import image22 from './img/team/image22.png';
import image23 from './img/team/image23.png';
import image24 from './img/team/image24.png';
import image25 from './img/team/image25.png';
import image26 from './img/team/image26.png';
import image27 from './img/team/image27.png';
import image28 from './img/team/image28.png';

function About() {

    return (
        <div className="about">
            <div className="about__wrapper">
                <div className="about__banner">
                    <div className="banner__wrapper">
                        <StarSvg></StarSvg>
                        <LogoMbtiSvg></LogoMbtiSvg>
                        <StarSvg></StarSvg>
                        <TextSvg></TextSvg>
                        <WaweSvg></WaweSvg>
                        <CogniSvg></CogniSvg>
                        <TypesSvg></TypesSvg>
                    </div>
                </div>

                <section className="about__description">
                <p className="about__description-p">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                </section>

                <section className="about__whatIs">
                    <p className="about__whatIs-p"><p className="whatIs__heading">Что такое MBTI?</p><br/>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                    <p>Чтобы узнать больше об MBTI, читайте нашу статью на вики! <a href="#">читать дальше</a></p>
                </section>

                <section className="about__COGNI">
                    <p className="about__COGNI-p"><p className="COGNI__heading">COGNI - проект Московского Политеха</p><br/>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                    <p>Чтобы узнать больше о Московском Политехе, переходите на наш сайт <a href="#">читать дальше</a></p>
                </section>

                <div className="about__banner about__banner--alt">
                    <div className="banner__wrapper banner__wrapper--alt">
                    <StarSvg></StarSvg>
                    <Text2Svg></Text2Svg>
                    <Wawe2Svg></Wawe2Svg>
                    <StarsSvg></StarsSvg>
                    </div>
                </div>

                <section className="about__leaders">
                    <ul className="leaders__list">
                        <li className="leaders__item">
                            <img src={image1} className="leaders__img"/>
                            <BorderSvg/><p className="leaders__name">Елена Ивановна Мещангина</p>
                            <span className="leaders__role">КУРАТОР ПРОЕКТА</span>
                        </li>
                        <li className="leaders__item">
                            <img src={image2} className="leaders__img"/>
                            <BorderSvg/> <p className="leaders__name">Кристина Кирксова</p>
                            <span className="leaders__role">ЛИДЕР ПРОЕКТА</span>
                        </li>
                        <li className="leaders__item">
                            <img src={image3} className="leaders__img"/>
                            <BorderSvg/><p className="leaders__name">Алина Мограбян</p>
                            <span className="leaders__role">МЕНЕДЖЕР ПРОЕКТА</span>
                        </li>
                    </ul>
                </section>

                <section className="team">
                    <div className="team__banner">
                        <p className="team__title">ДИЗАЙН</p>
                        <StafillSvg/>
                    </div>

                    <ul className="team__list">
                        <li className="team__item">
                            <img src={image4} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Камилла Касымова</p>
                        </li>

                        <li className="team__item">
                            <img src={image5} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Екатерина Батурина</p>
                        </li>

                        <li className="team__item">
                            <img src={image6} className="team__img"/>\
                            <Border2Svg/><p className="team__name">София Каткова</p>
                        </li>

                        <li className="team__item">
                            <img src={image7} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Анастасия Калинкина</p>
                        </li>

                        <li className="team__item">
                            <img src={image8} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Николай Булгаков</p>
                        </li>
                    </ul>
                </section>

                <section className="team">
                    <div className="team__banner">
                        <p className="team__title">РАБОТА С ТЕКСТОМ И АНАЛИЗ</p>
                        <StafillSvg/>
                    </div>

                    <ul className="team__list">
                        <li className="team__item">
                            <img src={image3} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Алина Мограбян</p>
                        </li>

                        <li className="team__item">
                            <img src={image9} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Алёна Швайкина</p>
                        </li>

                        <li className="team__item">
                            <img src={image10} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Анастасия Иванова</p>
                        </li>

                        <li className="team__item">
                            <img src={image11} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Алёна Комиссарова</p>
                        </li>

                        <li className="team__item">
                            <img src={image12} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Алина Ишунькина</p>
                        </li>

                        <li className="team__item">
                            <img src={image13} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Денис Житинский</p>
                        </li>

                        <li className="team__item">
                            <img src={image14} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Анастасия Квасова</p>
                        </li>
                    </ul>
                </section>

                <section className="team">
                    <div className="team__banner">
                        <p className="team__title">FRONTEND РАЗРАБОТКА</p>
                        <StafillSvg/>
                    </div>

                    <ul className="team__list">
                        <li className="team__item">
                            <img src={image15} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Илья Цымбал</p>
                        </li>

                        <li className="team__item">
                            <img src={image16} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Владислав Виноградов</p>
                        </li>

                        <li className="team__item">
                            <img src={image17} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Владислав Кашкин</p>
                        </li>

                        <li className="team__item">
                            <img src={image18} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Мария Степанчук</p>
                        </li>

                        <li className="team__item">
                            <img src={image19} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Рафаэл Гядукян</p>
                        </li>

                        <li className="team__item">
                            <img src={image20} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Улугбек Исматов</p>
                        </li>
                    </ul>
                </section>

                <section className="team">
                    <div className="team__banner">
                        <p className="team__title">BACKEND РАЗРАБОТКА</p>
                        <StafillSvg/>
                    </div>

                    <ul className="team__list">
                        <li className="team__item">
                            <img src={image21} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Анастасия Трушева</p>
                        </li>

                        <li className="team__item">
                            <img src={image22} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Анастасия Митяева</p>
                        </li>

                        <li className="team__item">
                            <img src={image23} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Артем Архипов</p>
                        </li>

                        <li className="team__item">
                            <img src={image24} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Вадим Потапов</p>
                        </li>

                        <li className="team__item">
                            <img src={image25} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Анастасия Бахматова</p>
                        </li>
                    </ul>
                </section>

                <section className="team">
                    <div className="team__banner">
                        <p className="team__title">РАЗРАБОТКА БАЗ ДАННЫХ</p>
                        <StafillSvg/>
                    </div>

                    <ul className="team__list">
                        <li className="team__item">
                            <img src={image26} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Анастасия Подгорная</p>
                        </li>

                        <li className="team__item">
                            <img src={image27} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Мартин Костюшкин</p>
                        </li>

                        <li className="team__item">
                            <img src={image28} className="team__img"/>\
                            <Border2Svg/><p className="team__name">Полина Голдина</p>
                        </li>
                    </ul>
                </section>
            </div>
        </div>
    );
};

export default About;


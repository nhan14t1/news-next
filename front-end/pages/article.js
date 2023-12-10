import React, { useState } from 'react';
import Head from 'next/head';
import getConfig from 'next/config';
import Image from 'next/legacy/image';
import dynamic from 'next/dynamic';
import styles from '../styles/Article.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendarAlt } from '@fortawesome/free-solid-svg-icons';
import { Button, ScrollToTop } from '../components';
import { appFetch } from '../shared/utils/apiUtils';
import { BASE_THUMBNAIL_URL, WEB_NAME } from '../shared/constants/app-const';
import * as moment from 'moment';

const Toast = dynamic(() => import('../components/Toast/Toast'));

function Article({ news }) {
  const ADD_BOOKMARK = 'Add Bookmark';
  const REMOVE_BOOKMARK = 'Remove Bookmark';

  const [bookmark, setBookmark] = useState(false);
  const [visible, setVisible] = useState(false);
  const [buttonText, setButtonText] = useState(ADD_BOOKMARK);

  const handleClick = () => {
    setVisible(true);
    setBookmark(!bookmark);
    setButtonText(!bookmark ? REMOVE_BOOKMARK : ADD_BOOKMARK);
  };

  const iconCalendar = <FontAwesomeIcon icon={faCalendarAlt} color='#3c3c3c' />;

  return (
    <div className='container'>
      <Head>
        <title>{`${news.title} - ${WEB_NAME}`}</title>
      </Head>

      <main>
        <article className={styles.wrapper}>
          <article className={styles.content_wrapper}>
            {/* <Button onClick={handleClick}>{buttonText}</Button> */}
            <p className={styles.date}>
              {iconCalendar} {moment(news.createdDate).format('DD/MM/YYYY')}
            </p>
            <h2>{news.title}</h2>
            <h4>{news.introText}</h4>
            <hr className={styles.divider} />
            <div
              className={styles.content}
              dangerouslySetInnerHTML={{ __html: news.content }}
            />
          </article>

          <article className={styles.media_wrapper}>
            {news.thumbnailFileName ? (
              <Image
                src={`${BASE_THUMBNAIL_URL}/${news.thumbnailFileName}`}
                alt='Article media'
                width={500}
                height={300}
              />
            ) : (
              <div className={styles.overlay}>
                <div className={styles.logo}></div>
              </div>
            )}

            <p className={styles.figcaption}>{news.introText}</p>
          </article>
        </article>
      </main>

      <Toast bookmark={bookmark} visible={visible} />
      <ScrollToTop />
    </div>
  );
}

Article.getInitialProps = async ({ query }) => {
  const { id } = query;
  const data = await appFetch(`/post/${id}`);
  if (!data) {
    return {
      notFound: true,
    };
  }

  return { news: data };
};

export default Article;

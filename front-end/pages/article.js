import React, { useEffect, useState } from 'react';
import Head from 'next/head';
import Image from 'next/legacy/image';
import dynamic from 'next/dynamic';
import styles from '../styles/Article.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendarAlt } from '@fortawesome/free-solid-svg-icons';
import { ScrollToTop } from '../components';
import { appFetch, put } from '../shared/utils/apiUtils';
import { BASE_THUMBNAIL_URL, WEB_NAME, BASE_URL } from '../shared/constants/app-const';
import * as moment from 'moment';

const Toast = dynamic(() => import('../components/Toast/Toast'));

function Article({ news, query }) {
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

  const countViews = () => {
    put(`/post/views/${news.id}`, null, true, false)
      .then(res => {})
      .catch(err => console.log(err));
  }

  useEffect(() => {
    countViews();
  }, [])

  const iconCalendar = <FontAwesomeIcon icon={faCalendarAlt} color='#3c3c3c' />;

  return (
    <div className='container'>
      <Head>
        <title>{`${news.title} - ${WEB_NAME}`}</title>
        <meta name="description" content={news.introText} />
        <meta property="og:url" content={`${BASE_URL}/article?id=${query.id}`} />
        <meta property="og:title" content={news.title} />
        <meta property="og:image" content={`${BASE_THUMBNAIL_URL}/${news.thumbnailFileName}`} />
        <meta property="og:description" content={news.introText} />
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
                alt={news.introText}
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

export async function getServerSideProps({query}) {
  const { id } = query;
  const data = await appFetch(`/post/${id}`);
  if (!data || data.isError) {
    return {
      notFound: true,
    };
  }

  return { props: { news: data, query } };
}

export default Article;

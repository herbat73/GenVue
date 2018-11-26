<template>
    <div>
        <div class="container">
            <v-layout row>
                <v-flex xs12 lg4 offset-lg4>
                    <div class="text-md-center">
                        <h5>Add new user to group {{ name }}</h5>
                        <v-layout row>
                            <v-select v-bind:items="items" 
                                      v-model="browseForUser"
                                      label="Select user"
                                      item-value="id"
                                      item-text="email">
                            </v-select>
                        </v-layout>
                    </div>
                </v-flex>
            </v-layout>
            <div class="text-xs-center">
                <v-btn outline color="indigo"
                       @click.native="addUserToGroup()">Add user to {{ name }}</v-btn>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'

    export default {
        props: {
            id: Number
        },

        data () {
        return {
            name: '',
            err: '',
            browseForUser : null,
            items: [],
          }
        },

        computed: {
        },

        methods: {
            getGroupData() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }
                    axios.get('/api/groups/' + this.id, config)
                        .then((res) => {
                            this.name = res.data.name
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },

            getUsers() {
                return new Promise((resolve, reject) => {

                    console.log('getUsers called.')

                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }

                    axios.get('/api/users', config)
                        .then((res) => {
                            //console.log('users res ' + res.data)
                            this.items = res.data
                        }).catch((err) => {
                            reject(err)
                        })
                })

            },

            addUserToGroup() {
                console.log('addUserToGroup group id ' + this.id + ' ' + this.browseForUser)
                return new Promise((resolve, reject) => {

                    const config = {
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                            'Authorization': 'Bearer ' + this.$store.getters.auth_data.token
                        },
                    }

                    const params = new URLSearchParams();
                    params.append('ApplicationUserId', this.browseForUser);
                    params.append('GroupId', this.id);

                    axios.post('/api/addusertogroup', params.toString(), config)
                        .then((res) => {
                            console.log('addusertogroup res status ' + res.status)
                            this.onSaveComplete()
                            //if (res.status !== 200) return reject(SpeercsErrors.RegistrationError())
                            resolve()
                        }).catch((err) => {
                            reject(err)
                        })
                })
               
            },

            onSaveComplete() {
                console.log('onSaveComplete')
                this.$router.push({ name: 'usersingroup', params: { id: this.id } })
            }
        },

        created() {
            if (this.id > 0) {
                this.getGroupData()
                this.getUsers()
            }
        },

    }
</script>




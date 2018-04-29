<template>
    <div>
        <div class="text-md-center">
            <v-card>
                <v-card-title>
                    User list
                    <v-spacer></v-spacer>
                    <v-text-field append-icon="search"
                                  label="Search"
                                  single-line
                                  hide-details
                                  v-model="search"></v-text-field>
                </v-card-title>
                <v-data-table v-bind:headers="headers"
                              v-bind:items="items"
                              v-bind:search="search">
                    <template slot="items" slot-scope="props">
                        <td class="text-xs-right">{{ props.item.email }}</td>
                        <td class="text-xs-right">{{ props.item.firstName }}</td>
                        <td class="text-xs-right">{{ props.item.lastName }}</td>
                    </template>
                    <template slot="pageText" slot-scope="{ pageStart, pageStop }">
                        From {{ pageStart }} to {{ pageStop }}
                    </template>
                </v-data-table>
            </v-card>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'

    export default {
        data() {
            return {
                search: '',
                pagination: {},
                headers: [
                    { text: 'email', value: 'email' },
                    { text: 'firstName', value: 'firstName' },
                    { text: 'lastName', value: 'lastName' }
                ],
                items: [],
            }
        },

        computed: {
        },


        methods: {
            getUsers() {
                return new Promise((resolve, reject) => {

                    console.log('getUsers called.')

                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }

                    axios.get('/api/users', config)
                        .then((res) => {
                            console.log('users res ' + res.data)
                            this.items = res.data
                        }).catch((err) => {
                            reject(err)
                        })
                })

            }
        },

        created() {
            this.getUsers()
        }
    }
</script>


